using TMPro;
using UnityEngine;

public class ChurchEditor : MonoBehaviour
{

    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private GameObject relocateMenu;
    [SerializeField] private GameObject rotateMenu;
    [SerializeField] private GameObject resizeMenu;
    [SerializeField] private ResolveController resolveController;
    [SerializeField] private SessionController controller;

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
        controller.SaveTransform(church.LocalPosition(), church.LocalEulerY(), church.Scale());
        GoBack();
    }
    public void GoBack()
    {
        DisableAllMenus();
        resolveController.EditDone();
    }

    public void SetChurch(Church church, ChurchAnchor churchAnchor)
    {
        this.church = church;
        church.ChurchSetUp(churchAnchor);
        UpdateText();
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

    private void UpdateText()
    {
        Vector3 localPos = church.LocalPosition();

        string x = localPos.x.ToString("F1");
        string y = localPos.y.ToString("F1");
        string z = localPos.z.ToString("F1");

        locationText.text = $"({x}.{y}.{z})";

        rotationText.text = church.LocalEulerY().ToString("F1");
        scaleText.text = church.Scale().ToString("F2");
    }

    #region Changing the church transform
    public void MoveLeft()
    {
        church.MoveLeft();
        UpdateText();
    }
    public void MoveRight()
    {
        church.MoveRight();
        UpdateText();
    }
    public void MoveUp()
    {
        church.MoveUp();
        UpdateText();
    }
    public void MoveDown()
    {
        church.MoveDown();
        UpdateText();
    }
    public void MoveForward()
    {
        church.MoveForward();
        UpdateText();
    }
    public void MoveBackwards()
    {
        church.MoveBackwards();
        UpdateText();
    }
    public void ScaleUp()
    {
        church.ScaleUp();
        UpdateText();
    }
    public void ScaleDown()
    {
        church.ScaleDown();
        UpdateText();
    }
    public void RotateLeft()
    {
        church.RotateLeft();
        UpdateText();
    }
    public void RotateRight()
    {
        church.RotateRight();
        UpdateText();
    }
    public void PressDone()
    {
        UpdateText();
        church.PressDone();
    }


    #endregion


}
