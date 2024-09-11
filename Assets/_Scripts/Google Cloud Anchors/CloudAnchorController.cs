using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using Unity.XR.CoreUtils;
using System;

public class CloudAnchorController : MonoBehaviour
{
    [Header("AR Foundation")]
    public XROrigin sessionOrigin;
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
    public HashSet<string> resolvingSet = new HashSet<string>();

    private const string persistentCloudAnchorsStorageKey = "PersistentCloudAnchors";
    private const int storageLimit = 40;

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
        resolvingSet.Clear();
        homeView.SetActive(true);
    }
    public void SwitchToARView()
    {
        ResetAllViews();
        arView.SetActive(true);
        //SetPlatformActive(true);
    }
    public void SwitchToResolveMenu()
    {
        ResetAllViews();
        resolveView.SetActive(true);  
    }

    private void ResetAllViews()
    {
        //SetPlatformActive(false);
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

    public StoredCloudAnchorCollection LoadCloudAnchorHistory()
    {
        if (PlayerPrefs.HasKey(persistentCloudAnchorsStorageKey))
        {
            StoredCloudAnchorCollection history = JsonUtility.FromJson<StoredCloudAnchorCollection>(PlayerPrefs.GetString(persistentCloudAnchorsStorageKey));

            // Remove all records created more than 24 hours and update stored history.
            DateTime current = DateTime.Now;
            history.collection.RemoveAll(data => current.Subtract(data.CreatedTime).Days > 0);
            PlayerPrefs.SetString(persistentCloudAnchorsStorageKey, JsonUtility.ToJson(history));

            return history;
        }

        return new StoredCloudAnchorCollection();
    }
    public void SaveCloudAnchorHistory(StoredCloudAnchor data)
    {
        StoredCloudAnchorCollection history = LoadCloudAnchorHistory();

        // Sort the data from latest record to oldest record which affects the option order in
        // multiselection dropdown.
        history.collection.Add(data);
        history.collection.Sort((left, right) => right.CreatedTime.CompareTo(left.CreatedTime));

        // Remove the oldest data if the capacity exceeds storage limit.
        if (history.collection.Count > storageLimit)
        {
            history.collection.RemoveRange(storageLimit, history.collection.Count - storageLimit);
        }

        PlayerPrefs.SetString(persistentCloudAnchorsStorageKey, JsonUtility.ToJson(history));
    }

    #endregion

    public void Awake()
    {
        SwitchToHomePage();
    }
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
