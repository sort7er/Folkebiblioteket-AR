using UnityEngine;

public class ChurchPrefab : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private Transform churchTransform;



    #region Changing the church transform
    public void MoveLeft()
    {
        float xPos = churchTransform.position.x;
        xPos -= movementSpeed * Time.deltaTime;

        churchTransform.position = new Vector3(xPos, churchTransform.position.y, churchTransform.position.z);
    }
    public void MoveRight() 
    {

        float xPos = churchTransform.position.x;
        xPos += movementSpeed * Time.deltaTime;

        churchTransform.position = new Vector3(xPos, churchTransform.position.y, churchTransform.position.z);
    }
    public void MoveUp()
    {
        float yPos = churchTransform.position.y;
        yPos += movementSpeed * Time.deltaTime;

        churchTransform.position = new Vector3(churchTransform.position.y, yPos, churchTransform.position.z);
    }
    public void MoveDown()
    {
        float yPos = churchTransform.position.y;
        yPos -= movementSpeed * Time.deltaTime;

        churchTransform.position = new Vector3(churchTransform.position.y, yPos, churchTransform.position.z);
    }
    public void MoveForward() 
    {
        float zPos = churchTransform.position.z;
        zPos += movementSpeed * Time.deltaTime;

        churchTransform.position = new Vector3(churchTransform.position.x, churchTransform.position.y, zPos);
    }
    public void MoveBackwards() 
    {
        float zPos = churchTransform.position.z;
        zPos -= movementSpeed * Time.deltaTime;

        churchTransform.position = new Vector3(churchTransform.position.x, churchTransform.position.y, zPos);
    }

    public void ScaleUp()
    {
        float currentScale = churchTransform.localScale.x;
        currentScale += movementSpeed * Time.deltaTime;

        churchTransform.localScale = Vector3.one * currentScale;
    }
    public void ScaleDown()
    {
        float currentScale = churchTransform.localScale.x;
        currentScale -= movementSpeed * Time.deltaTime;

        churchTransform.localScale = Vector3.one * currentScale;
    }

    #endregion

}
