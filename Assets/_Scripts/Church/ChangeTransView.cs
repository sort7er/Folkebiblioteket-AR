using UnityEngine;
using UnityEngine.UI;

public class ChangeTransView : MonoBehaviour
{

    public GameObject changeTransButton;
    public GameObject backButton;
    public GameObject selectChangeView;
    public GameObject relocateView;
    public GameObject resizeView;
    public GameObject rotateView;
    public ARViewLogic aRViewLogic;


    private Church church;


    private void Awake()
    {
        ShowChangeButton(false);
    }

    public void SetChurch(Church church)
    {
        this.church = church;
    }
    public void ChangeTransform()
    {
        ShowChangeButton(false);
        DisableAllViews();
        selectChangeView.SetActive(true);
        backButton.SetActive(false);
    }

    public void SaveChanges()
    {
        backButton.SetActive(true);
        ShowChangeButton(true);
        church.SaveTransform();
        DisableAllViews();

    }

    public void RelocateView()
    {
        DisableAllViews();
        relocateView.SetActive(true);
    }
    public void ResizeView()
    {
        DisableAllViews();
        resizeView.SetActive(true);
    }
    public void RotateView()
    {
        DisableAllViews();
        rotateView.SetActive(true);
    }

    private void DisableAllViews()
    {
        selectChangeView.SetActive(false);
        relocateView.SetActive(false);
        resizeView.SetActive(false);
        rotateView.SetActive(false);
    }

    public void ShowChangeButton(bool state)
    {
        changeTransButton.SetActive(state);
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
