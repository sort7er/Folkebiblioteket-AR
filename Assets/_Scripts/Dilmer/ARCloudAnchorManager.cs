using Google.XR.ARCoreExtensions;
using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCloudAnchorManager : MonoBehaviour
{
    public event Action<Transform> OnCloudAnchorCreated;

    public ARAnchorManager anchorManager;
    public Camera arCamera = null;
    public float resolvedAnchorPassedTimeout = 5;

    private ARAnchor pendingHostAnchor = null;
    private ARCloudAnchor cloudAnchor = null;
    private string anchorIdToResolve;
    private bool anchorHostInProgress = false;
    private bool anchorResolveInProgress = false;
    private float safeToResolvePassed = 0;

    private Pose GetCameraPose()
    {
        return new Pose(arCamera.transform.position, arCamera.transform.rotation);
    }

    public void QueueAnchor(ARAnchor arAnchor)
    {
        pendingHostAnchor = arAnchor;
    }
    public void HostAnchor()
    {
        Debug.Log("HostAnchor call in progress");
        FeatureMapQuality quality = anchorManager.EstimateFeatureMapQualityForHosting(GetCameraPose());

        Debug.Log($"Feature Map Quality is: {quality}");

        cloudAnchor = anchorManager.HostCloudAnchor(pendingHostAnchor, 1);

        if(cloudAnchor == null)
        {
            Debug.Log($"Unable to host cloud anchor: {pendingHostAnchor}");
        }
        else
        {
            anchorHostInProgress = true;
        }
    }
    public void Resolve()
    {
        Debug.Log("Resolve call in progress");

        cloudAnchor = anchorManager.ResolveCloudAnchorId(anchorIdToResolve);

        if (cloudAnchor == null)
        {
            Debug.Log($"Unable to resolve cloud anchor: {anchorIdToResolve}");
        }
        else
        {
            anchorResolveInProgress = true;
        }
    }
    private void CheckHostingProgress()
    {
        CloudAnchorState cloudAnchorState = cloudAnchor.cloudAnchorState;

        if(cloudAnchorState == CloudAnchorState.Success)
        {
            anchorHostInProgress = false;
            anchorIdToResolve = cloudAnchor.cloudAnchorId;
        }
        else if(cloudAnchorState != CloudAnchorState.TaskInProgress)
        {
            Debug.Log($"Error while hosting cloud anchor: {cloudAnchorState}");
            anchorHostInProgress = false;
        }
    }
    private void CheckResolveProgress()
    {
        CloudAnchorState cloudAnchorState = cloudAnchor.cloudAnchorState;

        if (cloudAnchorState == CloudAnchorState.Success)
        {
            OnCloudAnchorCreated?.Invoke(cloudAnchor.transform);
            anchorResolveInProgress = false;
        }
        else if (cloudAnchorState != CloudAnchorState.TaskInProgress)
        {
            Debug.Log($"Error while resolving cloud anchor: {cloudAnchorState}");
            anchorHostInProgress = false;
        }
    }
    private void Update()
    {
        //Cheking for host result
        if(anchorHostInProgress)
        {
            CheckHostingProgress();
            return;
        }


        //Cheking for resolve result
        if(anchorResolveInProgress && safeToResolvePassed <= 0)
        {
            safeToResolvePassed = resolvedAnchorPassedTimeout;

            if (!string.IsNullOrEmpty(anchorIdToResolve))
            {
                Debug.Log($"Resolving anchor ID: {anchorIdToResolve}");
                CheckResolveProgress();
            }
        }
        else
        {
            safeToResolvePassed -= Time.deltaTime * 1.0f;
        }
    }


}
