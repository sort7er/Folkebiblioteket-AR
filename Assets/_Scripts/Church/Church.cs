using System;
using UnityEngine;

public class Church : MonoBehaviour
{
    [SerializeField] private Transform churchTransform;
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



    #region Setting the church transform


    public void ChurchSetUp(ChurchAnchor church)
    {
        startColor = meshRenderer.material.color;

        Debug.Log("x " + church.localPosX);
        Debug.Log("y " + church.localPosY);
        Debug.Log("z " + church.localPosZ);
        Debug.Log("rotation " + church.localEulerY);
        Debug.Log("scale " + church.localScale);


        Vector3 localPos = new Vector3(church.localPosX, church.localPosY, church.localPosZ);
        churchTransform.localPosition = localPos;

        Vector3 localAngle = Vector3.up * church.localEulerY;
        churchTransform.localRotation = Quaternion.Euler(localAngle);

        Vector3 scale = Vector3.one * church.localScale;
        churchTransform.localScale = scale;

    }

    #endregion


    #region Changing the church transform
    public void MoveLeft()
    {
        float xPos = churchTransform.localPosition.x;
        xPos -= movementSpeed * Time.deltaTime;

        SetMaterialColor(xColor);


        churchTransform.localPosition = new Vector3(xPos, churchTransform.localPosition.y, churchTransform.localPosition.z);
    }
    public void MoveRight()
    {

        float xPos = churchTransform.localPosition.x;
        xPos += movementSpeed * Time.deltaTime;

        SetMaterialColor(xColor);

        churchTransform.localPosition = new Vector3(xPos, churchTransform.localPosition.y, churchTransform.localPosition.z);
    }
    public void MoveUp()
    {
        float yPos = churchTransform.localPosition.y;
        yPos += movementSpeed * Time.deltaTime;

        SetMaterialColor(yColor);


        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, yPos, churchTransform.localPosition.z);
    }
    public void MoveDown()
    {
        float yPos = churchTransform.localPosition.y;
        yPos -= movementSpeed * Time.deltaTime;

        SetMaterialColor(yColor);

        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, yPos, churchTransform.localPosition.z);
    }
    public void MoveForward()
    {
        float zPos = churchTransform.localPosition.z;
        zPos += movementSpeed * Time.deltaTime;


        SetMaterialColor(zColor);

        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, churchTransform.localPosition.y, zPos);
    }
    public void MoveBackwards()
    {
        float zPos = churchTransform.localPosition.z;
        zPos -= movementSpeed * Time.deltaTime;

        SetMaterialColor(zColor);

        churchTransform.localPosition = new Vector3(churchTransform.localPosition.x, churchTransform.localPosition.y, zPos);
    }

    public void ScaleUp()
    {
        float currentScale = churchTransform.localScale.x;
        currentScale += scaleSpeed * Time.deltaTime;

        SetMaterialColor(sizeUpColor);

        churchTransform.localScale = Vector3.one * currentScale;
    }
    public void ScaleDown()
    {
        float currentScale = churchTransform.localScale.x;
        currentScale -= scaleSpeed * Time.deltaTime;

        SetMaterialColor(sizeDownColor);

        churchTransform.localScale = Vector3.one * currentScale;
    }

    public void RotateLeft()
    {
        float currentRotation = churchTransform.localEulerAngles.y;
        currentRotation -= rotateSpeed * Time.deltaTime;

        SetMaterialColor(rotateLeftColor);

        churchTransform.localEulerAngles = Vector3.up * currentRotation;
    }
    public void RotateRight()
    {
        float currentRotation = churchTransform.localEulerAngles.y;
        currentRotation += rotateSpeed * Time.deltaTime;

        SetMaterialColor(rotateRightColor);

        churchTransform.localEulerAngles = Vector3.up * currentRotation;
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
    public Vector3 LocalPosition()
    {
        return churchTransform.localPosition;
    }
    public float LocalEulerY()
    {
        return churchTransform.localEulerAngles.y;
    }
    public float Scale()
    {
        return churchTransform.localScale.x;
    }


}
