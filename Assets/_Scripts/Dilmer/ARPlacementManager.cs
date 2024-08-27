using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class ARPlacementManager : MonoBehaviour
{
    public ARRaycastManager arRaycasterManager;
    public ARAnchorManager arAnchorManager;
    public ARCloudAnchorManager arCloudAnchorManager;
    public GameObject placedPrefab;

    private GameObject placedGameObject;

    private List<ARRaycastHit> hits = new();

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchPosition = touch.position;

                return true;

                //bool isOverUI = touchPosition.IsPointOverUIObject();

                //return isOverUI ? false : true;
            }
        }

        touchPosition = default;

        return false;
    }

    public void RemovePlacements()
    {
        Destroy(placedGameObject);
        placedGameObject = null;
    }
    private void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        if(placedGameObject != null)
        {
            return;
        }
        if(arRaycasterManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            placedGameObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
            var anchor = arAnchorManager.AddAnchor(new Pose(hitPose.position, hitPose.rotation));
            placedGameObject.transform.parent = anchor.transform;

            arCloudAnchorManager.QueueAnchor(anchor);
        }
    }
    public void ReCreatePlacement(Transform transform)
    {
        placedGameObject = Instantiate(placedPrefab, transform.position, transform.rotation);
        placedGameObject.transform.parent = transform;
    }
}
