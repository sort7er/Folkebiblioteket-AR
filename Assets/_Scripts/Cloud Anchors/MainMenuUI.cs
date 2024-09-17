using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject arSession;
    [SerializeField] private GameObject origin;
    [SerializeField] private GameObject extensions;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject hostView;
    [SerializeField] private GameObject editView;
    [SerializeField] private GameObject cantResolveMessage;
    [SerializeField] private GameObject enterStringView;
    [SerializeField] private GameObject editButton, copyIdButton;
    [SerializeField] private SessionController controller;


    private void Start()
    {
        AnchorCheck();
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
        if(controller.churchAnchor!= null)
        {
            ResetAllViews();
            EnableAR();
            editView.SetActive(true);
        }
        else
        {
            cantResolveMessage.SetActive(true);
        }
    }
    public void CantResolveMessageDone()
    {
        cantResolveMessage.SetActive(false);
    }
    public void MainMenu()
    {
        ResetAllViews();
        DisableAR();
        mainMenu.SetActive(true);
        AnchorCheck();
    }

    public void EnterAnchor()
    {
        ResetAllViews();
        enterStringView.SetActive(true);
    }
    public void CopyAnchor()
    {
        GUIUtility.systemCopyBuffer = controller.churchAnchor.GetChurchAnchorSting();
        Debug.Log("Copied string: " + controller.churchAnchor.GetChurchAnchorSting());
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
        enterStringView.SetActive(false);
    }
    private void AnchorCheck()
    {
        if(controller.churchAnchor != null)
        {
            editButton.SetActive(true);
            copyIdButton.SetActive(true);
        }
        else
        {
            editButton.SetActive(false);
            copyIdButton.SetActive(false);
        }
    }



}
