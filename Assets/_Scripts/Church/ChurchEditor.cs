using UnityEngine;

public class ChurchEditor : MonoBehaviour
{

    [SerializeField] private GameObject selectionMenu;
    [SerializeField] private GameObject relocateMenu;
    [SerializeField] private GameObject rotateMenu;
    [SerializeField] private GameObject resizeMenu;
    [SerializeField] private ResolveController resolveController;

    private Church church;

    private void Awake()
    {
        DisableAllMenus();
    }


    public void SaveAndGoBack()
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

        //Try and disable AR here later

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
        church.MoveLeft();
    }
    public void MoveRight()
    {
        church.MoveRight();
    }
    public void MoveUp()
    {
        church.MoveUp();
    }
    public void MoveDown()
    {
        church.MoveDown();
    }
    public void MoveForward()
    {
        church.MoveForward();
    }
    public void MoveBackwards()
    {
        church.MoveBackwards();
    }
    public void ScaleUp()
    {
        church.ScaleUp();
    }
    public void ScaleDown()
    {
        church.ScaleDown();
    }
    public void RotateLeft()
    {
        church.RotateLeft();
    }
    public void RotateRight()
    {
        church.RotateRight();
    }
    public void PressDone()
    {
        church.PressDone();
    }

    #endregion


}
