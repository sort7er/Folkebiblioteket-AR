using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

public class ARCloudAnchorExperienceManager : MonoBehaviour
{
    public UnityEvent OnInitialized;
    public UnityEvent OnRestarted;
    public float maxScanningAreaTime = 30;


    public ARPlaneManager arPlaneManager;
    public ARPointCloudManager arPointCloudManager;

    private bool Initialized { get; set; }

    private bool AllowCloudAnchorDelay { get; set; }

    private float timePassedAfterPlanesDetected = 0;



    void Awake()
    {
        arPlaneManager.planesChanged += PlanesChanged;
    }


    void Update()
    {
        if (AllowCloudAnchorDelay)
        {
            if (timePassedAfterPlanesDetected <= maxScanningAreaTime)
            {
                timePassedAfterPlanesDetected += Time.deltaTime * 1.0f;
                Debug.Log($"Experience starts in {maxScanningAreaTime - timePassedAfterPlanesDetected} sec(s)");
            }
            else
            {
                timePassedAfterPlanesDetected = maxScanningAreaTime;
                Activate();
            }
        }
    }

    void PlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!Initialized)
        {
            AllowCloudAnchorDelay = true;
        }
    }

    private void Activate()
    {

        Debug.Log("Activate AR Cloud Anchor Experience");
        OnInitialized?.Invoke();
        Initialized = true;
        AllowCloudAnchorDelay = false;
        arPlaneManager.enabled = false;
        arPointCloudManager.enabled = false;
    }

    public void Restart()
    {
        Debug.Log("Restart AR Cloud Anchor Experience");
        OnRestarted?.Invoke();
        Initialized = false;
        AllowCloudAnchorDelay = true;
        arPlaneManager.enabled = true;
        arPointCloudManager.enabled = true;
    }
}
