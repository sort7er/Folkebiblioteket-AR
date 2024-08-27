using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnAnchorFromRay : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARAnchorManager anchorManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private Camera arCamera;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);

            if (raycastManager.Raycast(ray, hits, TrackableType.Planes))
            {
                SpawnAnchor(planeManager.GetPlane(hits[0].trackableId), hits[0].pose);
            }

        }
    }

    public void SpawnAnchor(ARPlane plane, Pose pose)
    {
        Pose hitPose = new Pose(pose.position, Quaternion.LookRotation(-pose.up));

        var result = anchorManager.AttachAnchor(plane, pose);
    }
}
