using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class ARDrawManager : MonoBehaviour
{
    public float distanceFromCamera = 0.3f;
    public float lineWidth = 0.05f;
    public int cornerVertices = 5;
    public int endCapVertices = 5;
    public Material defaultColorMaterial;

    public bool allowSimplification = false;
    public float tolerance = 0.001f;
    public float applySimplifyAfterPoints = 20.0f;
    [Range(0f, 1.0f)]
    public float minDistanceBeforeNewPoint = 0.01f;

    public UnityEvent OnDraw;
    public ARAnchorManager anchorManager;
    public Camera arCamera;
    public Color defaultColor = Color.white;


    private LineRenderer currentLineRender;
    private List<ARAnchor> anchors = new List<ARAnchor>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    private int positionCount = 0;
    private Vector3 prevPointDistance = Vector3.zero;
    private bool CanDraw { get; set; }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            DrawOnTouch();
        }

    }
    public void AllowDraw(bool isAllowed)
    {
        CanDraw = isAllowed;
    }
    private void DrawOnTouch()
    {
        if (!CanDraw) return;

        Touch touch = Input.GetTouch(0);
        Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distanceFromCamera));

        if(touch.phase == TouchPhase.Began)
        {
            OnDraw?.Invoke();

            ARAnchor anchor = anchorManager.AddAnchor(new Pose(touchPosition, Quaternion.identity));

            if (anchor == null)
            {
                Debug.Log("Error creating reference point");
            }
            else
            {
                anchors.Add(anchor);
            }
            AddNewLineRenderer(anchor, touchPosition);
        }
        else
        {
            UpdateLine(touchPosition);
        }
    }
    private void UpdateLine(Vector3 touchPosition)
    {
        if(prevPointDistance == null)
        {
            prevPointDistance = touchPosition;
        }
        if(prevPointDistance != null && Mathf.Abs(Vector3.Distance(prevPointDistance, touchPosition)) >= minDistanceBeforeNewPoint)
        {
            prevPointDistance = touchPosition;
            AddPoint(prevPointDistance);
        }
    }
    private void AddPoint(Vector3 position)
    {
        positionCount++;
        currentLineRender.positionCount= positionCount;

        currentLineRender.SetPosition(positionCount - 1, position);

        if(currentLineRender.positionCount % applySimplifyAfterPoints == 0 && allowSimplification)
        {
            currentLineRender.Simplify(tolerance);
        }
    }
    private void AddNewLineRenderer(ARAnchor arAnchor, Vector3 touchPosition)
    {
        positionCount = 2;
        GameObject go = new GameObject($"LineRenderer_{lines.Count}");

        go.transform.parent = arAnchor?.transform ?? transform;
        go.transform.position = touchPosition;
        go.tag = "Line";
        LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();



        SetLineSettings(goLineRenderer, touchPosition);


        currentLineRender = goLineRenderer;

        lines.Add(goLineRenderer);
    }
    private void SetLineSettings(LineRenderer currentLineRenderer, Vector3 touchPosition)
    {

        currentLineRenderer.material = defaultColorMaterial;
        currentLineRenderer.useWorldSpace = true;
        currentLineRenderer.positionCount = positionCount;
        currentLineRenderer.startWidth = lineWidth;
        currentLineRenderer.endWidth = lineWidth;
        currentLineRenderer.numCornerVertices = cornerVertices;
        currentLineRenderer.numCapVertices = endCapVertices;
        if (allowSimplification) currentLineRenderer.Simplify(tolerance);
        currentLineRenderer.startColor = defaultColor;
        currentLineRenderer.endColor = defaultColor;
        currentLineRenderer.SetPosition(0, touchPosition);
        currentLineRenderer.SetPosition(1, touchPosition);
    }

    private GameObject[] GetAllLinesInScene()
    {
        return GameObject.FindGameObjectsWithTag("Line");
    }

    public void ClearLines()
    {
        GameObject[] lines = GetAllLinesInScene();
        foreach (GameObject go in lines)
        {
            Destroy(go);
        }
    }
}
