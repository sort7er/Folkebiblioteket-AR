using TMPro;
using UnityEngine;

public class ChurchEditor : MonoBehaviour
{

    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private GameObject relocateMenu;
    [SerializeField] private GameObject rotateMenu;
    [SerializeField] private GameObject resizeMenu;
    [SerializeField] private ResolveController resolveController;

    [SerializeField] private TextMeshProUGUI locationText;
    [SerializeField] private TextMeshProUGUI rotationText;
    [SerializeField] private TextMeshProUGUI scaleText;

    private Church church;

    private void Awake()
    {
        DisableAllMenus();
    }


    public void SaveAndGoBack()
    {
        GoBack();
    }
    public void GoBack()
    {
        DisableAllMenus();
        resolveController.EditDone();
    }

    public void SetChurch(Church church)
    {
        this.church = church;
    }

    public void SelectionMenu()
    {
        DisableAllMenus();
        selectionMenu.SetActive(true);

    }

    public void Relocate()
    {
        DisableAllMenus();
        relocateMenu.SetActive(true);
    }
    public void Rotate()
    {
        DisableAllMenus();
        rotateMenu.SetActive(true);
    }
    public void Resize()
    {
        DisableAllMenus();
        resizeMenu.SetActive(true);

    }
    private void DisableAllMenus()
    {
        relocateMenu.SetActive(false);
        rotateMenu.SetActive(false);
        resizeMenu.SetActive(false);
        selectionMenu.SetActive(false);
    }
    #region Changing the church transform
    public void MoveLeft()
    {
        UpdatePosition(church.MoveLeft());
    }
    public void MoveRight()
    {
        UpdatePosition(church.MoveRight());
    }
    public void MoveUp()
    {
        UpdatePosition(church.MoveUp());
    }
    public void MoveDown()
    {
        UpdatePosition(church.MoveDown());
    }
    public void MoveForward()
    {
        UpdatePosition(church.MoveForward());
    }
    public void MoveBackwards()
    {
        UpdatePosition(church.MoveBackwards());
    }
    public void ScaleUp()
    {
        UpdateScale(church.ScaleUp());
    }
    public void ScaleDown()
    {
        UpdateScale(church.ScaleDown());
    }
    public void RotateLeft()
    {
        UpdateRotatiom(church.RotateLeft());
    }
    public void RotateRight()
    {
        UpdateRotatiom(church.RotateRight());
    }
    public void PressDone()
    {
        church.PressDone();
    }

    private void UpdatePosition(Vector3 localPosition)
    {
        string x = localPosition.x.ToString("F1");
        string y = localPosition.y.ToString("F1");
        string z = localPosition.z.ToString("F1");

        locationText.text = $"({x},{y},{z})";
    }
    private void UpdateRotatiom(float rotation)
    {
        rotationText.text = rotation.ToString("F1");
    }

    private void UpdateScale(float scale)
    {
        scaleText.text = scale.ToString("F1");
    }

    #endregion


}
