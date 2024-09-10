
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class MapQualityIndicator : MonoBehaviour
{
    public MapQualityBar mapQualityBarPrefab;
    //public CircleRenderer circleRenderer;
    [Range(0,360)]
    public float range = 150;
    public float radius = 0.1f;

    /// The indication range for vertical surface.
    private const float verticalRange = 150.0f;

    /// The indication range for horizontal surface.
    private const float horizontalRange = 180.0f;

    /// The quality threshold which indicates it's sufficient to host an anchor.
    private const float qualityThreshold = 0.6f;

    /// The angle threshold that indicates the camera is looking from the top view of this indicator.
    private const float topviewThreshold = 15.0f;

    private const float disappearDuration = 0.5f;
    private const float fadingDuration = 3.0f;
    private const float barSpacing = 7.5f;
    private const float circleFadingRange = 10.0f;

    private int currentQualityState = 0;
    private Camera mainCamera = null;
    private Vector3 centerDir;
    private float fadingTimer = -1.0f;
    private float disappearTimer = -1.0f;
    private List<MapQualityBar> mapQualityBars = new List<MapQualityBar>();

    public bool ReachQualityThreshold
    {
        get
        {
            float currentQuality = 0.0f;
            foreach (var bar in mapQualityBars)
            {
                currentQuality += bar.Weight;
            }

            return (currentQuality / mapQualityBars.Count) >= qualityThreshold;
        }
    }
    public bool ReachTopviewAngle
    {
        get
        {
            var cameraDir = mainCamera.transform.position - transform.position;
            return Vector3.Angle(cameraDir, Vector3.up) < topviewThreshold;
        }
    }
    public void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Update()
    {
        // Play fading animation.
        if (ReachTopviewAngle)
        {
            // Fading animation finished.
            if (fadingTimer >= fadingDuration)
            {
                return;
            }

            // Start fading animation.
            if (fadingTimer < 0)
            {
                fadingTimer = 0.0f;
            }

            fadingTimer += Time.deltaTime;
            SetAplha();

            return;
        }
        else if (fadingTimer > 0)
        {
            fadingTimer -= Time.deltaTime;
            SetAplha();
        }

        // Update visited bar.
        for (int i = 0; i < mapQualityBars.Count; i++)
        {
            if (IsLookingAtBar(mapQualityBars[i]))
            {
                mapQualityBars[i].IsVisited = true;
                mapQualityBars[i].QualityState = currentQualityState;
            }
        }

        PlayDisappearAnimation();
    }

    private void SetAplha()
    {
        float alpha = Mathf.Clamp(1 - (fadingTimer / fadingDuration), 0, 1);

        //CircleRenderer.SetAlpha(alpha);
        for (int i = 0; i < mapQualityBars.Count; i++)
        {
            mapQualityBars[i].SetAlpha(alpha);
        }
    }

    public void UpdateQualityState(int quality)
    {
        currentQualityState = quality;
    }
    public void DrawIndicator(PlaneAlignment planeAlignment, Camera camera)
    {
        // To use customized value, remove this line and set the desired range in inspector.
        range = planeAlignment == PlaneAlignment.Vertical ? verticalRange : horizontalRange;

        mainCamera = camera;

        // Get the direction from the center of the circle to the center of the arc
        // in world space.
        centerDir = planeAlignment == PlaneAlignment.Vertical ?
            transform.TransformVector(Vector3.up) :
            transform.TransformVector(-Vector3.forward);

        DrawBars();
        DrawRing();

        gameObject.SetActive(true);
    }

    private void DrawRing()
    {
        //CircleRenderer.DrawArc(_centerDir, Radius, Range + (2 * _circleFadingRange));
    }

    private void DrawBars()
    {
        // Place a quality bar at the center position.

        Vector3 basePos = transform.position;
        Vector3 position = centerDir * radius;
        var rotation = Quaternion.AngleAxis(0, Vector3.up);

        AddBar(basePos, position, rotation);

        for (float deltaAngle = barSpacing; deltaAngle < range / 2; deltaAngle += barSpacing)
        {
            // Place a quality bar at the left and right side.
            DrawBar(deltaAngle, basePos);
            DrawBar(-deltaAngle, basePos);
        }
    }

    private void DrawBar(float angle, Vector3 basePos)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 position = rotation * centerDir * radius;
        AddBar(basePos, position, rotation);
    }
    private void AddBar(Vector3 basePos, Vector3 position, Quaternion rotation)
    {
        MapQualityBar qualityBar = Instantiate(mapQualityBarPrefab, basePos + position, rotation, transform);
        mapQualityBars.Add(qualityBar);
    }
    private bool IsLookingAtBar(MapQualityBar bar)
    {
        // Check whether the bar is inside camera's view:
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(bar.transform.position);
        if (screenPoint.z <= 0 || screenPoint.x <= 0 || screenPoint.x >= 1 || screenPoint.y <= 0 || screenPoint.y >= 1)
        {
            return false;
        }

        // Check the distance between the indicator and the camera.
        float distance = (transform.position - mainCamera.transform.position).magnitude;
        if (distance <= radius)
        {
            return false;
        }

        Vector3 cameraDir = Vector3.ProjectOnPlane(mainCamera.transform.position - transform.position, Vector3.up);
        Vector3 barDir = Vector3.ProjectOnPlane(bar.transform.position - transform.position, Vector3.up);

        return Vector3.Angle(cameraDir, barDir) < barSpacing;
    }
    private void PlayDisappearAnimation()
    {
        if (disappearTimer < 0.0f && ReachQualityThreshold)
        {
            disappearTimer = 0.0f;
        }

        if (disappearTimer >= 0.0f && disappearTimer < disappearDuration)
        {
            disappearTimer += Time.deltaTime;
            float scale = Mathf.Max(0.0f, (disappearDuration - disappearTimer) / disappearDuration);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        if (disappearTimer >= disappearDuration)
        {
            gameObject.SetActive(false);
        }
    }
   
}
