using UnityEngine.XR.ARFoundation;
using UnityEngine;
using Unity.XR.CoreUtils;

public class CloudAnchorController : MonoBehaviour
{
    [Header("AR Foundation")]
    public ARSessionOrigin sessionOrigin;
    public ARSession session;
    public GameObject extensions;
    public ARAnchorManager anchorManager;
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;

    [Header("UI")]
    public GameObject homeView;
    public GameObject resolveView;
    public GameObject arView;

    [HideInInspector] public ApplicationMode mode = ApplicationMode.Ready;

    private const string persistentCloudAnchorIdKey = "anchorId";


    public enum ApplicationMode
    {
        Ready,
        Hosting,
        Resolving,
    }
    public Camera MainCamera
    {
        get
        {
            return sessionOrigin.Camera;
        }
    }
    public void Awake()
    {
        SwitchToHomePage();


    }


    #region Buttons
    public void OnHostButtonClicked()
    {
        mode = ApplicationMode.Hosting;
        SwitchToARView();
    }
    public void OnResolveButtonClicked()
    {
        mode = ApplicationMode.Resolving;
        SwitchToResolveMenu();
    }

    #endregion

    #region Views
    public void SwitchToHomePage()
    {
        ResetAllViews();
        mode = ApplicationMode.Ready;
        homeView.SetActive(true);
    }
    public void SwitchToARView()
    {
        ResetAllViews();
        arView.SetActive(true);
        SetPlatformActive(true);
    }
    public void SwitchToResolveMenu()
    {
        ResetAllViews();
        resolveView.SetActive(true);  
    }

    private void ResetAllViews()
    {
        SetPlatformActive(false);
        arView.SetActive(false);
        resolveView.SetActive(false);
        homeView.SetActive(false);
    }

    private void SetPlatformActive(bool active)
    {
        sessionOrigin.gameObject.SetActive(active);
        session.gameObject.SetActive(active);
        extensions.SetActive(active);
    }
    #endregion

    #region CloudAnchorHistory

    public string LoadCurrentCloudAnchorId()
    {
        if (PlayerPrefs.HasKey(persistentCloudAnchorIdKey))
        {
            return PlayerPrefs.GetString(persistentCloudAnchorIdKey);
        }
        else
        {
            return null;
        }
    }

    public void SaveCurrentCloudAnchorId(string newId)
    {
        PlayerPrefs.SetString(persistentCloudAnchorIdKey, newId);
    }

    #endregion


    //public void Update()
    //{

    //    // On home page, pressing 'back' button quits the app.
    //    // Otherwise, returns to home page.
    //    if (Input.GetKeyUp(KeyCode.Escape))
    //    {
    //        if (homeView.activeSelf)
    //        {
    //            Application.Quit();
    //        }
    //        else
    //        {
    //            SwitchToHomePage();
    //        }
    //    }
    //}


}
