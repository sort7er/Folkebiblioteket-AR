using UnityEngine;

public class Church : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float scaleSpeed = 0.5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private Transform churchTransform;
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
    public void MoveLeft()
    {
        float xPos = churchTransform.localPosition.x;
        xPos -= movementSpeed * Time.deltaTime;

        meshRenderer.material.color = xColor;

        churchTransform.localPosition = new Vector3(xPos, churchTransform.localPosition.y, churchTransform.localPosition.z);
    }
    public void MoveRight() 
    {

        float xPos = churchTransform.localPosition.x;
        xPos += movementSpeed * Time.deltaTime;

        meshRenderer.material.color = xColor;

        churchTransform.localPosition = new Vector3(xPos, churchTransform.localPosition.y, churchTransform.localPosition.z);
    }
    public void MoveUp()
    {
        float yPos = churchTransform.localPosition.y;
        yPos += movementSpeed * Time.deltaTime;

        meshRenderer.material.color = yColor;


        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, yPos, churchTransform.localPosition.z);
    }
    public void MoveDown()
    {
        float yPos = churchTransform.localPosition.y;
        yPos -= movementSpeed * Time.deltaTime;

        meshRenderer.material.color = yColor;

        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, yPos, churchTransform.localPosition.z);
    }
    public void MoveForward() 
    {
        float zPos = churchTransform.localPosition.z;
        zPos += movementSpeed * Time.deltaTime;


        meshRenderer.material.color = zColor;

        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, churchTransform.localPosition.y, zPos);
    }
    public void MoveBackwards() 
    {
        float zPos = churchTransform.localPosition.z;
        zPos -= movementSpeed * Time.deltaTime;

        meshRenderer.material.color = zColor;

        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, churchTransform.localPosition.y, zPos);
    }

    public void ScaleUp()
    {
        float currentScale = churchTransform.localScale.x;
        currentScale += scaleSpeed * Time.deltaTime;

        meshRenderer.material.color = sizeUpColor;

        churchTransform.localScale = Vector3.one * currentScale;
    }
    public void ScaleDown()
    {
        float currentScale = churchTransform.localScale.x;
        currentScale -= scaleSpeed * Time.deltaTime;

        meshRenderer.material.color = sizeDownColor;

        churchTransform.localScale = Vector3.one * currentScale;
    }

    public void RotateLeft()
    {
        float currentRotation = churchTransform.localEulerAngles.y;
        currentRotation -= rotateSpeed * Time.deltaTime;

        meshRenderer.material.color = rotateLeftColor;

        churchTransform.localEulerAngles = Vector3.up * currentRotation;
    }
    public void RotateRight()
    {
        float currentRotation = churchTransform.localEulerAngles.y;
        currentRotation += rotateSpeed * Time.deltaTime;

        meshRenderer.material.color = rotateRightColor;

        churchTransform.localEulerAngles = Vector3.up * currentRotation;
    }

    public void PressDone()
    {

        Debug.Log("donee");

        meshRenderer.material.color = startColor;
    }

    #endregion

}
