using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Initialize : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARAnchor anchoredPrefab;
    [SerializeField] private GameObject defaultPrefab;

    private void Awake()
    {
        planeManager.planesChanged += PlanesChanged;
    }

    private void PlanesChanged(ARPlanesChangedEventArgs args)
    {
        planeManager.planesChanged -= PlanesChanged;
        planeManager.enabled = false;
        
        ARAnchor ancored = Instantiate(anchoredPrefab);
        ancored.enabled = false;

        Instantiate(defaultPrefab);


    }

}
