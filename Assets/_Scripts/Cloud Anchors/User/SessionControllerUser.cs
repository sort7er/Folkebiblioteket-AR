using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SessionControllerUser : MonoBehaviour
{
    [SerializeField] private MainMenuUser mainMenuUI;
    public ChurchAnchor churchAnchor { get; private set; }
    public bool isReturning { get; private set; }

    public Church churchPrefab;
    public Camera mainCamera;
    public ARPlaneManager planeManager;
    public ARAnchorManager anchorManager;

    public void CreateCloudAnchor(string name, string id, Vector3 localPosition, float localEulerY, float localScale)
    {
        churchAnchor = new ChurchAnchor(name, id);

        churchAnchor.SetLocalPosition(localPosition);
        churchAnchor.SetLocalEulerY(localEulerY);
        churchAnchor.SetLocalScale(localScale);
    }
    public void UpdatePlaneVisibility(bool visible)
    {
        planeManager.enabled = visible;

        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(visible);
        }
    }
    public void SetIsReturning(bool state)
    {
        isReturning = state;
    }
    public void CheckDoAndNull<T>(ref T type, Action thingToDo = null) where T : class
    {
        if (type != null)
        {
            thingToDo?.Invoke();
            type = null;
        }
    }
    public bool ErrorCheckAndDisableSleep()
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
            return true;
        }

        // Return to home page if ARSession is in error status.
        if (ARSession.state != ARSessionState.Ready && ARSession.state != ARSessionState.SessionInitializing && ARSession.state != ARSessionState.SessionTracking)
        {
            string reason = $"ARCore encountered an error state {ARSession.state}. Please start the app again.";
            Debug.Log("Returning home for reason: " + reason);

            if (isReturning)
            {
                return true;
            }

            SetIsReturning(true);
            Invoke(nameof(DoReturnToHomePage), 3.0f);
        }

        return false;
    }
    private void DoReturnToHomePage()
    {
        mainMenuUI.OnMainMenu();
    }
}
