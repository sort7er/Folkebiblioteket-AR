using UnityEngine;

public class Church : MonoBehaviour
{
    public Transform churchTransform;
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float scaleSpeed = 0.5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Color xColor;
    [SerializeField] private Color yColor;
    [SerializeField] private Color zColor;
    [SerializeField] private Color sizeUpColor;
    [SerializeField] private Color sizeDownColor;
    [SerializeField] private Color rotateLeftColor;
    [SerializeField] private Color rotateRightColor;

    private Color startColor;


    private const string xLocalPos = "xLocalPos";
    private const string yLocalPos = "yLocalPos";
    private const string zLocalPos = "zLocalPos";
    private const string yLocalAngle = "yLocalAngle";
    private const string localScale = "localScale";


    private void Awake()
    {
        startColor = meshRenderer.material.color;
        CheckTransform();
    }

    #region Setting the church transform

    private void CheckTransform()
    {
        if (PlayerPrefs.HasKey(xLocalPos))
        {
            Vector3 localPos = new Vector3(PlayerPrefs.GetFloat(xLocalPos), PlayerPrefs.GetFloat(yLocalPos), PlayerPrefs.GetFloat(zLocalPos));
            churchTransform.localPosition = localPos;
        }

        if (PlayerPrefs.HasKey(yLocalAngle))
        {
            Vector3 localAngle = Vector3.up * PlayerPrefs.GetFloat(yLocalAngle);
            churchTransform.localRotation = Quaternion.Euler(localAngle);
        }


        if (PlayerPrefs.HasKey(localScale))
        {
            Vector3 scale = Vector3.one * PlayerPrefs.GetFloat(localScale);
            churchTransform.localScale = scale;
        }
    }

    #endregion


    public void SaveTransform()
    {
        PlayerPrefs.SetFloat(xLocalPos, churchTransform.localPosition.x);
        PlayerPrefs.SetFloat(yLocalPos, churchTransform.localPosition.y);
        PlayerPrefs.SetFloat(zLocalPos, churchTransform.localPosition.z);

        PlayerPrefs.SetFloat(yLocalAngle, churchTransform.localEulerAngles.y);
        PlayerPrefs.SetFloat(localScale, churchTransform.localScale.x);
    }


    #region Changing the church transform
    public Vector3 MoveLeft()
    {
        float xPos = churchTransform.localPosition.x;
        xPos -= movementSpeed * Time.deltaTime;

        SetMaterialColor(xColor);


        churchTransform.localPosition = new Vector3(xPos, churchTransform.localPosition.y, churchTransform.localPosition.z);
        Debug.Log(churchTransform.localPosition.x);

        return churchTransform.localPosition;
    }
    public Vector3 MoveRight()
    {

        float xPos = churchTransform.localPosition.x;
        xPos += movementSpeed * Time.deltaTime;

        SetMaterialColor(xColor);

        churchTransform.localPosition = new Vector3(xPos, churchTransform.localPosition.y, churchTransform.localPosition.z);
        return churchTransform.localPosition;
    }
    public Vector3 MoveUp()
    {
        float yPos = churchTransform.localPosition.y;
        yPos += movementSpeed * Time.deltaTime;

        SetMaterialColor(yColor);


        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, yPos, churchTransform.localPosition.z);
        return churchTransform.localPosition;
    }
    public Vector3 MoveDown()
    {
        float yPos = churchTransform.localPosition.y;
        yPos -= movementSpeed * Time.deltaTime;

        SetMaterialColor(yColor);

        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, yPos, churchTransform.localPosition.z);
        return churchTransform.localPosition;
    }
    public Vector3 MoveForward()
    {
        float zPos = churchTransform.localPosition.z;
        zPos += movementSpeed * Time.deltaTime;


        SetMaterialColor(zColor);

        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, churchTransform.localPosition.y, zPos);
        return churchTransform.localPosition;
    }
    public Vector3 MoveBackwards()
    {
        float zPos = churchTransform.localPosition.z;
        zPos -= movementSpeed * Time.deltaTime;

        SetMaterialColor(zColor);

        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, churchTransform.localPosition.y, zPos);
        return churchTransform.localPosition;
    }

    public float ScaleUp()
    {
        float currentScale = churchTransform.localScale.x;
        currentScale += scaleSpeed * Time.deltaTime;

        SetMaterialColor(sizeUpColor);

        churchTransform.localScale = Vector3.one * currentScale;
        return churchTransform.localScale.x;
    }
    public float ScaleDown()
    {
        float currentScale = churchTransform.localScale.x;
        currentScale -= scaleSpeed * Time.deltaTime;

        SetMaterialColor(sizeDownColor);

        churchTransform.localScale = Vector3.one * currentScale;
        return churchTransform.localScale.x;
    }

    public float RotateLeft()
    {
        float currentRotation = churchTransform.localEulerAngles.y;
        currentRotation -= rotateSpeed * Time.deltaTime;

        SetMaterialColor(rotateLeftColor);

        churchTransform.localEulerAngles = Vector3.up * currentRotation;
        return churchTransform.localEulerAngles.y;
    }
    public float RotateRight()
    {
        float currentRotation = churchTransform.localEulerAngles.y;
        currentRotation += rotateSpeed * Time.deltaTime;

        SetMaterialColor(rotateRightColor);

        churchTransform.localEulerAngles = Vector3.up * currentRotation;

        return churchTransform.localEulerAngles.y;
    }

    public void PressDone()
    {
        SetMaterialColor(startColor);
    }

    #endregion

    private void SetMaterialColor(Color color)
    {
        for(int i = 0; i< meshRenderer.materials.Length; i++)
        {
            meshRenderer.materials[i].color = color;
        }
    }

}
