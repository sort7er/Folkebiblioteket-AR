using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject arSession;
    [SerializeField] private GameObject origin;
    [SerializeField] private GameObject extensions;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject hostView;
    [SerializeField] private GameObject editView;




    #region Buttons
    public void Host()
    {
        ResetAllViews();
        EnableAR();
        hostView.SetActive(true);
    }

    public void EditChurch()
    {
        ResetAllViews();
        EnableAR();
        editView.SetActive(true);
    }
    public void MainMenu()
    {
        ResetAllViews();
        DisableAR();
        mainMenu.SetActive(true);
    }

    #endregion

    private void Awake()
    {
        DisableAR();
    }
    public void EnableAR()
    {
        origin.SetActive(true);
        arSession.SetActive(true);
        extensions.SetActive(true);
    }
    public void DisableAR()
    {
        origin.SetActive(false);
        arSession.SetActive(false);
        extensions.SetActive(false);
    }

    private void ResetAllViews()
    {
        mainMenu.SetActive(false);
        hostView.SetActive(false);
        editView.SetActive(false);
    }
}
