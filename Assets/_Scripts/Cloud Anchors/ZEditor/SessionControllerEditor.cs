using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SessionControllerEditor : MonoBehaviour
{
    [SerializeField] private MainMenuEditor mainMenuUI;

    public Church churchPrefab;
    public Camera mainCamera;
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    public ARAnchorManager anchorManager;


    public bool isReturning { get; private set; }
    public ChurchAnchor churchAnchor { get; private set; }

    public float startPrepareTime { get; private set; } = 10f;
    public float timeSinceStart { get; private set; } = 0f;


    private const string idKey = "idKey";
    private const string nameKey = "nameKey";
    private const string xLocalPos = "xLocalPos";
    private const string yLocalPos = "yLocalPos";
    private const string zLocalPos = "zLocalPos";
    private const string yLocalAngle = "yLocalAngle";
    private const string localScale = "localScale";

    private void Awake()
    {
        if (PlayerPrefs.HasKey(idKey))
        {
            SaveCurrentCloudAnchorId(PlayerPrefs.GetString(nameKey), PlayerPrefs.GetString(idKey));

            if (PlayerPrefs.HasKey(xLocalPos) && PlayerPrefs.HasKey(yLocalPos) && PlayerPrefs.HasKey(zLocalPos))
            {
                float x = PlayerPrefs.GetFloat(xLocalPos);
                float y = PlayerPrefs.GetFloat(yLocalPos);
                float z = PlayerPrefs.GetFloat(zLocalPos);

                churchAnchor.SetLocalPosition(new Vector3(x, y, z));
                Debug.Log("Position recovered");
            }
            if (PlayerPrefs.HasKey(yLocalAngle))
            {
                churchAnchor.SetLocalEulerY(PlayerPrefs.GetFloat(yLocalAngle));
                Debug.Log("Rotation recovered");
            }
            if (PlayerPrefs.HasKey(localScale))
            {
                churchAnchor.SetLocalScale(PlayerPrefs.GetFloat(localScale));
                Debug.Log("Scale recovered");
            }

        }
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
        mainMenuUI.MainMenu();
    }
    public Pose GetCameraPose()
    {
        return new Pose(mainCamera.transform.position, mainCamera.transform.rotation);
    }
    public void SaveCurrentCloudAnchorId(string name, string id)
    {
        if(churchAnchor == null)
        {
            churchAnchor = new ChurchAnchor(name, id);
        }
        else
        {
            churchAnchor.name = name;
            churchAnchor.id = id;
        }

        PlayerPrefs.SetString(nameKey, name);
        PlayerPrefs.SetString(idKey, id);
    }
    public void SaveTransform(Vector3 localPosition, float eulerY, float scale)
    {
        PlayerPrefs.SetFloat(xLocalPos, localPosition.x);
        PlayerPrefs.SetFloat(yLocalPos, localPosition.y);
        PlayerPrefs.SetFloat(zLocalPos, localPosition.z);

        PlayerPrefs.SetFloat(yLocalAngle, eulerY);
        PlayerPrefs.SetFloat(localScale, scale);

        churchAnchor.SetLocalPosition(localPosition);
        churchAnchor.SetLocalEulerY(eulerY);
        churchAnchor.SetLocalScale(scale);
    }

    public void CheckDoAndNull<T>(ref T type, Action thingToDo = null) where T : class
    {
        if (type != null)
        {
            thingToDo?.Invoke();
            type = null;
        }
    }

    public void IncreaseTimeSinceStart()
    {
        timeSinceStart += Time.deltaTime;
    }

}
