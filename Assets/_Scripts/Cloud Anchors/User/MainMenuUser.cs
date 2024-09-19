using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MainMenuUser : MonoBehaviour
{
    [SerializeField] private GameObject session;
    [SerializeField] private GameObject arOrigin;
    [SerializeField] private GameObject extensions;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject arView;
    [SerializeField] private GameObject warningMessage;


    public void Warning()
    {
        warningMessage.SetActive(true);
    }

    public void OnStart()
    {
        DisableMenu();
        EnableAR();
        arView.SetActive(true);
    }
    public void OnMainMenu()
    {
        DisableMenu();
        DisableAR();
        mainMenu.SetActive(true);
    }

    private void DisableMenu()
    {
        mainMenu.SetActive(false);
        arView.SetActive(false);
    }
    private void EnableAR()
    {
        arOrigin.SetActive(true);
        session.SetActive(true);
        //Extensions needs to be activa last
        extensions.SetActive(true);
    }
    private void DisableAR()
    {
        arOrigin.SetActive(false);
        session.SetActive(false);
        extensions.SetActive(false);
    }

}
