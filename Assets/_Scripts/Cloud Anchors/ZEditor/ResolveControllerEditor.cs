using Google.XR.ARCoreExtensions;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ResolveControllerEditor : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private GameObject instructionbar;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject editButton;
    [SerializeField] private SessionControllerEditor controller;
    [SerializeField] private ChurchEditor churchEditor;

    private ResolveCloudAnchorPromise resolvePromise;
    private ResolveCloudAnchorResult resolveResult;


    private void OnEnable()
    {
        resolvePromise = null;
        resolveResult = null;


        SetEditButton(false);

        controller.SetIsReturning(false);
        controller.UpdatePlaneVisibility(true);
    }

    private void OnDisable()
    {
        controller.CheckDoAndNull(ref resolvePromise, () => resolvePromise.Cancel());

        if (resolveResult != null && resolveResult.Anchor != null)
        {
            Destroy(resolveResult.Anchor.gameObject);
        }

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
                SetInstructionText("Look at the location you expect to see the AR experience appear.");
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

        ResolvingCloudAnchors();

    }

    private void ResolvingCloudAnchors()
    {
        // No Cloud Anchor for resolving.

        if (controller.churchAnchor == null)
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



        resolvePromise = controller.anchorManager.ResolveCloudAnchorAsync(controller.churchAnchor.id);


        if (resolvePromise.State == PromiseState.Done)
        {
            Debug.Log("Failed to resolve Cloud Anchor " + controller.churchAnchor.id);
            ResolveFailed(controller.churchAnchor.id);
        }
        else
        {
            var coroutine = ResolveAnchor(controller.churchAnchor.id, resolvePromise);
            StartCoroutine(coroutine);
        }
    }
    private IEnumerator ResolveAnchor(string cloudId, ResolveCloudAnchorPromise promise)
    {
        yield return promise;


        resolveResult = promise.Result;
        resolvePromise = null;

        if (resolveResult.CloudAnchorState == CloudAnchorState.Success)
        {
            SetInstructionText("Resolve success!");
            Debug.Log($"Succeed to resolve the Cloud Anchor: {cloudId}.");


            Church church = Instantiate(controller.churchPrefab, resolveResult.Anchor.transform);

            churchEditor.SetChurch(church, controller.churchAnchor);
            SetEditButton(true);
        }
        else
        {
            ResolveFailed(cloudId, resolveResult.CloudAnchorState.ToString());
        }
        controller.UpdatePlaneVisibility(false);
    }

    private void ResolveFailed(string id, string response = null)
    {
        SetInstructionText("Resolve failed.");
        Debug.Log("Failed to resolve Cloud Anchor: " + id + (response == null ? "." : "with error " + response + "."));
    }

    private void SetEditButton(bool state)
    {
        editButton.SetActive(state);
    }

    public void SwitchToEdit()
    {
        SetEditButton(false);
        backButton.SetActive(false);
        instructionbar.SetActive(false);

        churchEditor.SelectionMenu();

    }

    public void EditDone()
    {
        SetEditButton(true);
        backButton.SetActive(true); 
        instructionbar.SetActive(true);
    }


}
