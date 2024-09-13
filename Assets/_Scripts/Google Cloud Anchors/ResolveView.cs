using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolveView : MonoBehaviour
{
    public CloudAnchorController controller;
    public ARViewLogic aRViewLogic;
    public TextMeshProUGUI idText;
    public Button resolveButton;
    public Image resolveButtonImage;

    public GameObject copyButton;
    public GameObject enterIdButton;
    public TMP_InputField inputField;

    private Color activeColor;
    private Regex regex;

    private void Awake()
    {
        regex = new Regex("^[a-zA-Z0-9-_]*$");
        resolveButtonImage = resolveButton.GetComponent<Image>();
        activeColor = resolveButtonImage.color;
    }

    private void SetButtonActive(bool active)
    {
        resolveButtonImage.color = active ? activeColor : Color.grey;
        resolveButton.enabled = active;
    }
    public void OnEnable()
    {
        if(controller.LoadCurrentCloudAnchorId() == null)
        {
            SetButtonActive(false);
            idText.text = "No anchor to resolve";
            copyButton.SetActive(false);
        }
        else
        {
            SetButtonActive(true);
            idText.text = controller.LoadCurrentCloudAnchorId();
            copyButton.SetActive(true);
        }

        enterIdButton.SetActive(true);
        inputField.gameObject.SetActive(false);
        inputField.text = string.Empty;
    }
    public void EnterIdManually()
    {
        enterIdButton.SetActive(false);
        inputField.gameObject.SetActive(true);
        inputField.text = string.Empty;
    }
    public void OnInputValueChanged(string id)
    {
        if(IDCheck(id))
        {
            SetButtonActive(true);
        }
    }

    public void ResolvePressed()
    {
        if (IDCheck(inputField.text))
        {
            aRViewLogic.SetAnchorID(inputField.text);
        }

        controller.SwitchToARView();
    }
    private bool IDCheck(string id)
    {
        if(id.Length > 0 && regex.IsMatch(id))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
