using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class ARExperienceManager : MonoBehaviour
{
    public event Action OnPlaneFound;
    public event Action OnDelete;


    public ARPlaneManager arPlaneManager;

    public UnityEvent OnInitialize;
    public UnityEvent OnRestarted;

    private bool Initialized { get; set; }

    private void Awake()
    {
        arPlaneManager.planesChanged += PlanesChanged;
    }

    private void PlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!Initialized)
        {
            Activate();
        }
    }
    private void Activate()
    {
        OnInitialize?.Invoke();
        OnPlaneFound?.Invoke();
        Initialized = true;
        arPlaneManager.enabled = false;
    }
    public void Restart()
    {
        OnRestarted?.Invoke();
        OnDelete?.Invoke();
        Initialized = false;
        arPlaneManager.enabled = true;
    }
    
}
