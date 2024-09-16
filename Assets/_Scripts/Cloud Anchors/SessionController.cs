using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.ARFoundation;

public class SessionController : MonoBehaviour
{
    [SerializeField] private GameObject arSession;
    [SerializeField] private GameObject origin;
    [SerializeField] private MainMenuUI mainMenuUI;

    public GameObject churchPrefab;
    public Camera mainCamera;
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    public ARAnchorManager anchorManager;

    public bool isReturning { get; private set; }


    private void Awake()
    {
        DisableAR();
    }
    public void EnableAR()
    {
        origin.SetActive(true);
        arSession.SetActive(true);
    }
    public void DisableAR()
    {
        origin.SetActive(false);
        arSession.SetActive(false);
    }
    public void SetIsReturning(bool state)
    {
        isReturning = state;
    }

    public void UpdatePlaneVisibility(bool visible)
    {
        planeManager.enabled = visible;

        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(visible);
        }
    }

    public void ErrorCheckAndDisableSleep()
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
            string reason = $"ARCore encountered an error state {ARSession.state}. Please start the app again.";
            Debug.Log("Returning home for reason: " + reason);

            if (isReturning)
            {
                return;
            }

            SetIsReturning(true);
            Invoke(nameof(DoReturnToHomePage), 3.0f);
        }
    }

    private void DoReturnToHomePage()
    {
        mainMenuUI.MainMenu();
    }
    public Pose GetCameraPose()
    {
        return new Pose(mainCamera.transform.position, mainCamera.transform.rotation);
    }
}
