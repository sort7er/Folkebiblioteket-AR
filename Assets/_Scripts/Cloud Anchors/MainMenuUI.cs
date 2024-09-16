using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
   
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject hostView;
    [SerializeField] private GameObject editView;

    [SerializeField] private SessionController controller;




    #region Buttons
    public void Host()
    {
        ResetAllViews();
        controller.EnableAR();
        hostView.SetActive(true);
    }

    public void EditChurch()
    {
        ResetAllViews();
        controller.EnableAR();
        editView.SetActive(true);
    }
    public void MainMenu()
    {
        ResetAllViews();
        controller.DisableAR();
        mainMenu.SetActive(true);
    }

    #endregion



    private void ResetAllViews()
    {
        mainMenu.SetActive(false);
        hostView.SetActive(false);
        editView.SetActive(false);
    }
}
