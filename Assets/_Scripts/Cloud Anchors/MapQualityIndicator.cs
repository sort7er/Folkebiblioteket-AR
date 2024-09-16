using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.ARSubsystems;

public class MapQualityIndicator : MonoBehaviour
{

    public float radius;

        public bool ReachQualityThreshold
    {
        get
        {
            return true;
            //float currentQuality = 0.0f;
            //foreach (var bar in mapQualityBars)
            //{
            //    currentQuality += bar.Weight;
            //}

            //return (currentQuality / mapQualityBars.Count) >= qualityThreshold;
        }
    }
    public bool ReachTopviewAngle
    {
        get
        {
            return true;
            //var cameraDir = mainCamera.transform.position - transform.position;
            //return Vector3.Angle(cameraDir, Vector3.up) < topviewThreshold;
        }
    }

    public void DrawIndicator(PlaneAlignment planeType, Camera cam)
    {

    }

    public void UpdateQualityState(int quality)
    {

    }


}
