using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HostingController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private SessionControllerEditor controller;
    [SerializeField] private MapQualityIndicator mapQualityIndicatorPrefab;

    //Can't be more than one unless the project is not using an API key, then it is 365 days
    [SerializeField] private int lifetimeOfAnchorInDays = 1;
    [SerializeField] private GameObject enterNamePanel;
    [SerializeField] private TMP_InputField nameField;




    private HostCloudAnchorPromise hostPromise;
    private HostCloudAnchorResult hostResult;
    private IEnumerator hostCoroutine;
    private MapQualityIndicator mapQualityIndicator = null;
    private ARAnchor anchor;

    private void OnEnable()
    {
        anchor = null;
        mapQualityIndicator = null;
        hostPromise = null;
        hostResult = null;
        hostCoroutine = null;

        controller.SetIsReturning(false);
        controller.UpdatePlaneVisibility(true);

    }

    private void OnDisable()
    {
        controller.CheckDoAndNull(ref mapQualityIndicator, () => Destroy(mapQualityIndicator.gameObject));
        controller.CheckDoAndNull(ref anchor, () => Destroy(anchor.gameObject));
        controller.CheckDoAndNull(ref hostCoroutine, () => StopCoroutine(hostCoroutine));
        controller.CheckDoAndNull(ref hostPromise, () => hostPromise.Cancel());
        controller.CheckDoAndNull(ref hostResult);

        controller.UpdatePlaneVisibility(false);
    }

    private void SetInstructionText(string text)
    {
        instructionText.text = text;
    }
    private void Update()
    {
        if (controller.timeSinceStart < controller.startPrepareTime)
        {
            SetInstructionText("Move around the room");

            controller.IncreaseTimeSinceStart();

            if (controller.timeSinceStart >= controller.startPrepareTime)
            {
                SetInstructionText("Tap to place an object.");
            }

            return;
        }

        if (controller.ErrorCheckAndDisableSleep())
        {
            return;
        }

        if (controller.isReturning)
        {
            return;
        }


        // Perform hit test and place an anchor on the hit test result.
        if (anchor == null)
        {

            if (Input.GetMouseButtonDown(0))
            {
                PerformHitTest(Input.mousePosition);
            }
            else
            {
                // If the player has not touched the screen then the update is complete.
                Touch touch;
                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                {
                    return;
                }

                // Ignore the touch if it's pointing on UI objects.
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return;
                }

                // Perform hit test and place a pawn object.
                PerformHitTest(touch.position);
            }
        }

        HostingCloudAnchor();
    }

    private void PerformHitTest(Vector2 touchPos)
    {
        List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
        controller.raycastManager.Raycast(touchPos, hitResults, TrackableType.PlaneWithinPolygon);

        // If there was an anchor placed, then instantiate the corresponding object.
        var planeType = PlaneAlignment.HorizontalUp;

        if (hitResults.Count > 0)
        {
            ARPlane plane = controller.planeManager.GetPlane(hitResults[0].trackableId);
            if (plane == null)
            {
                Debug.Log($"Failed to find the ARPlane with TrackableId {hitResults[0].trackableId}");
                return;
            }

            planeType = plane.alignment;
            var hitPose = hitResults[0].pose;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // Point the hitPose rotation roughly away from the raycast/camera
                // to match ARCore.
                hitPose.rotation.eulerAngles = new Vector3(0.0f, controller.mainCamera.transform.eulerAngles.y, 0.0f);
            }

            anchor = controller.anchorManager.AttachAnchor(plane, hitPose);
        }

        if (anchor != null)
        {
            Instantiate(controller.churchPrefab, anchor.transform);

            // Attach map quality indicator to this anchor.
            mapQualityIndicator = Instantiate(mapQualityIndicatorPrefab, anchor.transform);
            mapQualityIndicator.DrawIndicator(planeType, controller.mainCamera);

            SetInstructionText("To save this location, walk around the object to capture it from different angles");

            Debug.Log("Waiting for sufficient mapping quaility...");


            // Hide plane generator so users can focus on the object they placed.
            controller.UpdatePlaneVisibility(false);
        }
    }

    private void HostingCloudAnchor()
    {
        // There is no anchor for hosting.
        if (anchor == null)
        {
            return;
        }

        // There is a pending or finished hosting task.
        if (hostPromise != null || hostResult != null)
        {
            return;
        }

        // Update map quality:
        int qualityState = 2;

        // Can pass in ANY valid camera pose to the mapping quality API.
        // Ideally, the pose should represent users’ expected perspectives.

        FeatureMapQuality quality = controller.anchorManager.EstimateFeatureMapQualityForHosting(controller.GetCameraPose());
        Debug.Log("Current mapping quality: " + quality);
        qualityState = (int)quality;

        mapQualityIndicator.UpdateQualityState(qualityState);

        // Hosting instructions:
        var cameraDist = (mapQualityIndicator.transform.position - controller.mainCamera.transform.position).magnitude;

        if (cameraDist < mapQualityIndicator.radius * 1.5f)
        {
            SetInstructionText("You are too close, move backward.");
            return;
        }
        else if (cameraDist > 10.0f)
        {
            SetInstructionText("You are too far, come closer.");
            return;
        }
        else if (mapQualityIndicator.ReachTopviewAngle)
        {
            SetInstructionText("You are looking from the top view, move around from all sides.");
            return;
        }
        else if (!mapQualityIndicator.ReachQualityThreshold)
        {
            SetInstructionText("Save the object here by capturing it from all sides.");
            return;
        }

        // Start hosting:
        SetInstructionText("Processing...");
        Debug.Log("Mapping quality has reached sufficient threshold, creating Cloud Anchor.");
        Debug.Log($"FeatureMapQuality has reached {controller.anchorManager.EstimateFeatureMapQualityForHosting(controller.GetCameraPose())}, triggering CreateCloudAnchor.");

        // Creating a Cloud Anchor with lifetime = 1 day.
        // This is configurable up to 365 days when keyless authentication is used.
        HostCloudAnchorPromise promise = controller.anchorManager.HostCloudAnchorAsync(anchor, lifetimeOfAnchorInDays);
        
        if (promise.State == PromiseState.Done)
        {
            Debug.Log("Failed to host a Cloud Anchor.");
            HostFailed();
        }
        else
        {
            hostPromise = promise;
            hostCoroutine = HostAnchor();
            StartCoroutine(hostCoroutine);
        }
    }

    private IEnumerator HostAnchor()
    {
        yield return hostPromise;
        hostResult = hostPromise.Result;
        hostPromise = null;

        if (hostResult.CloudAnchorState == CloudAnchorState.Success)
        {
            OpenNamePanel();
        }
        else
        {
            HostFailed(hostResult.CloudAnchorState.ToString());
        }
    }
    private void OpenNamePanel()
    {
        enterNamePanel.SetActive(true);
    }
    public void Finished()
    {
        enterNamePanel.SetActive(false);

        string name = nameField.text;

        if(name == "")
        {
            name = "Unamed anchor";
        }

        string id = hostResult.CloudAnchorId;

        SetInstructionText("Finished!");
        controller.SaveCurrentCloudAnchorId(name, id);
        controller.SaveTransform(Vector3.forward * 20, 270, 1);
        Debug.Log($"Succeed to host the Cloud Anchor: {id}");
    }


    private void HostFailed(string response = null)
    {
        SetInstructionText("Host failed.");
        Debug.Log("Failed to host a Cloud Anchor" + (response == null ? "." : "with error " + response + "."));
    }

}
