using UnityEngine;

public class SessionController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject hostView;
    public GameObject editView;

    public GameObject arSession;
    public GameObject origin;

    private void Awake()
    {
        DisableAR();
    }

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

    public void EnableAR()
    {
        origin.SetActive(true);
        arSession.SetActive(true);
    }
    public void DisableAR()
    {
        origin.SetActive(false);
        arSession.SetActive(false);
    }

    private void ResetAllViews()
    {
        mainMenu.SetActive(false);
        hostView.SetActive(false);
        editView.SetActive(false);
    }




}
