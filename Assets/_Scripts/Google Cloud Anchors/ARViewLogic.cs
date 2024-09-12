using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARViewLogic : MonoBehaviour
{
    public CloudAnchorController controller;
    public ARViewUI uiHandler;
    public Church churchPrefab;
    public MapQualityIndicator mapQualityIndicatorPrefab;
    public ChangeTransView changeTrans;




    private const float startPrepareTime = 3.0f;
    private const string pixelModel = "pixel";

    private float timeSinceStart;
    private bool isReturning;
    private ARAnchor anchor = null;
    private MapQualityIndicator mapQualityIndicator = null;
    private HostCloudAnchorPromise hostPromise;
    private HostCloudAnchorResult hostResult;
    private IEnumerator hostCoroutine;

    private ResolveCloudAnchorPromise resolvePromise;
    private ResolveCloudAnchorResult resolveResult;

    //private List<IEnumerator> resolveCoroutines = new List<IEnumerator>();

    private string anchorId;



    public Pose GetCameraPose()
    {
        return new Pose(controller.MainCamera.transform.position, controller.MainCamera.transform.rotation);
    }


    #region Buttons

    public void OnShareButtonClicked()
    {
        GUIUtility.systemCopyBuffer = anchorId;
        Debug.Log("Copied cloud id: " + anchorId);
    }

    #endregion
    
    #region OnEnable and OnDisable
    public void OnEnable()
    {
        timeSinceStart = 0.0f;
        isReturning = false;
        anchor = null;
        mapQualityIndicator = null;
        hostPromise = null;
        hostResult = null;
        hostCoroutine = null;
        resolvePromise = null;
        resolveResult = null;

        uiHandler.WhenEnabled();

        UpdatePlaneVisibility(true);

        switch (controller.mode)
        {
            case CloudAnchorController.ApplicationMode.Ready:
                ReturnToHomePage("Invalid application mode, returning to home page...");
                break;
            case CloudAnchorController.ApplicationMode.Hosting:
            case CloudAnchorController.ApplicationMode.Resolving:
                uiHandler.SetInstructionText("Detecting flat surface...");
                Debug.Log("ARCore is preparing for " + controller.mode);
                break;
        }
    }

    public void OnDisable()
    {
        // debug this afterwards

        if (mapQualityIndicator != null)
        {
            Destroy(mapQualityIndicator.gameObject);
            mapQualityIndicator = null;
        }

        if (anchor != null)
        {

            Destroy(anchor.gameObject);
            anchor = null;
        }

        if (hostCoroutine != null)
        {

            StopCoroutine(hostCoroutine);
            hostCoroutine = null;
        }


        if (hostPromise != null)
        {
            hostPromise.Cancel();
            hostPromise = null;
        }

        if (hostResult != null)
        {
            hostResult = null;
        }

        if(resolvePromise != null)
        {
            resolvePromise.Cancel();
            resolvePromise = null;
        }

        if(resolveResult != null && resolveResult.Anchor != null)
        {
            Destroy(resolveResult.Anchor.gameObject);
        }

        changeTrans.ShowChangeButton(false);

        UpdatePlaneVisibility(false);
    }
    #endregion

    public void Update()
    {

        // Give ARCore some time to prepare for hosting or resolving.
        if (timeSinceStart < startPrepareTime)
        {
            timeSinceStart += Time.deltaTime;
            if (timeSinceStart >= startPrepareTime)
            {
                UpdateInitialInstruction();
            }

            return;
        }

        ARCoreLifecycleUpdate();
        if (isReturning)
        {
            return;
        }

        if (timeSinceStart >= startPrepareTime)
        {
            uiHandler.DisplayTrackingHelperMessage(isReturning);
        }

        if (controller.mode == CloudAnchorController.ApplicationMode.Resolving)
        {
            ResolvingCloudAnchors();
        }
        else if (controller.mode == CloudAnchorController.ApplicationMode.Hosting)
        {
            // Perform hit test and place an anchor on the hit test result.
            if (anchor == null)
            {

                if(Input.GetMouseButtonDown(0))
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
    }
    #region Update related methods
    private void UpdateInitialInstruction()
    {
        switch (controller.mode)
        {
            case CloudAnchorController.ApplicationMode.Hosting:
                uiHandler.SetInstructionText("Tap to place an object.");
                Debuger.Instance.SendMessage("Tap a vertical or horizontal plane...");
                return;

            case CloudAnchorController.ApplicationMode.Resolving:
                uiHandler.SetInstructionText("Look at the location you expect to see the AR experience appear.");
                Debuger.Instance.SendMessage($"Attempting to resolve anchor...");
                return;

            default:
                return;
        }
    }
    private void ARCoreLifecycleUpdate()
    {
        // Only allow the screen to sleep when not tracking.
        var sleepTimeout = SleepTimeout.NeverSleep;
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            sleepTimeout = SleepTimeout.SystemSetting;
        }

        Screen.sleepTimeout = sleepTimeout;

        if (isReturning)
        {
            return;
        }

        // Return to home page if ARSession is in error status.
        if (ARSession.state != ARSessionState.Ready && ARSession.state != ARSessionState.SessionInitializing && ARSession.state != ARSessionState.SessionTracking)
        {
            ReturnToHomePage($"ARCore encountered an error state {ARSession.state}. Please start the app again.");
        }
    }
    private void ResolvingCloudAnchors()
    {
        // No Cloud Anchor for resolving.

        if (controller.LoadCurrentCloudAnchorId() == null)
        {
            return;
        }

        // There are pending or finished resolving tasks.
        if (resolvePromise != null || resolveResult != null)
        {
            return;
        }

        // ARCore session is not ready for resolving.
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            return;
        }



        resolvePromise = controller.anchorManager.ResolveCloudAnchorAsync(controller.LoadCurrentCloudAnchorId());
        

        if (resolvePromise.State == PromiseState.Done)
        {
            Debug.Log("Failed to resolve Cloud Anchor " + anchorId);
            OnAnchorResolvedFinished(false, anchorId);
        }
        else
        {
            var coroutine = ResolveAnchor(anchorId, resolvePromise);
            StartCoroutine(coroutine);
        }
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
                hitPose.rotation.eulerAngles = new Vector3(0.0f, controller.MainCamera.transform.eulerAngles.y, 0.0f);
            }
            
            anchor = controller.anchorManager.AttachAnchor(plane, hitPose);
        }

        if (anchor != null)
        {
            Instantiate(churchPrefab, anchor.transform);

            // Attach map quality indicator to this anchor.
            mapQualityIndicator = Instantiate(mapQualityIndicatorPrefab, anchor.transform);
            mapQualityIndicator.DrawIndicator(planeType, controller.MainCamera);

            uiHandler.SetInstructionText("To save this location, walk around the object to capture it from different angles");

            Debug.Log("Waiting for sufficient mapping quaility...");


            // Hide plane generator so users can focus on the object they placed.
            UpdatePlaneVisibility(false);
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

        FeatureMapQuality quality = controller.anchorManager.EstimateFeatureMapQualityForHosting(GetCameraPose());
        Debug.Log("Current mapping quality: " + quality);
        qualityState = (int)quality;

        mapQualityIndicator.UpdateQualityState(qualityState);

        // Hosting instructions:
        var cameraDist = (mapQualityIndicator.transform.position - controller.MainCamera.transform.position).magnitude;

        if (cameraDist < mapQualityIndicator.radius * 1.5f)
        {
            uiHandler.SetInstructionText("You are too close, move backward.");
            return;
        }
        else if (cameraDist > 10.0f)
        {
            uiHandler.SetInstructionText("You are too far, come closer.");
            return;
        }
        else if (mapQualityIndicator.ReachTopviewAngle)
        {
            uiHandler.SetInstructionText("You are looking from the top view, move around from all sides.");
            return;
        }
        else if (!mapQualityIndicator.ReachQualityThreshold)
        {
            uiHandler.SetInstructionText("Save the object here by capturing it from all sides.");
            return;
        }

        // Start hosting:
        uiHandler.SetInstructionText("Processing...");
        Debug.Log("Mapping quality has reached sufficient threshold, " + "creating Cloud Anchor.");
        Debug.Log($"FeatureMapQuality has reached {controller.anchorManager.EstimateFeatureMapQualityForHosting(GetCameraPose())}, triggering CreateCloudAnchor.");

        // Creating a Cloud Anchor with lifetime = 1 day.
        // This is configurable up to 365 days when keyless authentication is used.
        var promise = controller.anchorManager.HostCloudAnchorAsync(anchor, 1);
        if (promise.State == PromiseState.Done)
        {
            Debug.Log("Failed to host a Cloud Anchor.");
            OnAnchorHostedFinished(false);
        }
        else
        {
            hostPromise = promise;
            hostCoroutine = HostAnchor();
            StartCoroutine(hostCoroutine);
        }
    }
    #endregion

    #region Coroutines

    private IEnumerator HostAnchor()
    {
        yield return hostPromise;
        hostResult = hostPromise.Result;
        hostPromise = null;

        if (hostResult.CloudAnchorState == CloudAnchorState.Success)
        {
            anchorId = hostResult.CloudAnchorId;
            OnAnchorHostedFinished(true, anchorId);
        }
        else
        {
            OnAnchorHostedFinished(false, hostResult.CloudAnchorState.ToString());
        }
    }
    private IEnumerator ResolveAnchor(string cloudId, ResolveCloudAnchorPromise promise)
    {
        yield return promise;


        resolveResult = promise.Result;      
        resolvePromise = null;

        if (resolveResult.CloudAnchorState == CloudAnchorState.Success)
        {
            OnAnchorResolvedFinished(true, cloudId);
            Church church = Instantiate(churchPrefab, resolveResult.Anchor.transform);

            changeTrans.SetChurch(church);
            changeTrans.ShowChangeButton(true);
        }
        else
        {
            OnAnchorResolvedFinished(false, cloudId, resolveResult.CloudAnchorState.ToString());
        }
        UpdatePlaneVisibility(false);
    }
    #endregion

    #region When host or resolve is finished
    private void OnAnchorHostedFinished(bool success, string response = null)
    {
        if (success)
        {
            uiHandler.SetInstructionText("Finish!");
            uiHandler.SuccessfullHosting(anchorId);
            controller.SaveCurrentCloudAnchorId(anchorId);
            Debug.Log($"Succeed to host the Cloud Anchor: {response}");
        }
        else
        {
            uiHandler.SetInstructionText("Host failed.");
            Debug.Log("Failed to host a Cloud Anchor" + (response == null ? "." : "with error " + response + "."));
        }
    }
    private void OnAnchorResolvedFinished(bool success, string cloudId, string response = null)
    {
        if (success)
        {
            uiHandler.SetInstructionText("Resolve success!");
            Debug.Log($"Succeed to resolve the Cloud Anchor: {cloudId}.");
        }
        else
        {
            uiHandler.SetInstructionText("Resolve failed.");
            Debug.Log("Failed to resolve Cloud Anchor: " + cloudId + (response == null ? "." : "with error " + response + "."));
        }
    }
    #endregion

    private void UpdatePlaneVisibility(bool visible)
    {
        //controller.planeManager.enabled= visible;
        //foreach (ARPlane plane in controller.planeManager.trackables)
        //{
        //    plane.gameObject.SetActive(visible);
        //}
    }
  
    private void ReturnToHomePage(string reason)
    {
        Debug.Log("Returning home for reason: " + reason);
        if (isReturning)
        {
            return;
        }

        isReturning = true;
        Invoke(nameof(DoReturnToHomePage), 3.0f);
    }



    private void DoReturnToHomePage()
    {
        controller.SwitchToHomePage();
    }
}
