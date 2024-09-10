using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARViewUI : MonoBehaviour
{
    public GameObject instructionBar;
    public GameObject namePanel;
    public GameObject inputFieldWarning;

    public TMP_InputField nameField;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI trackingHelperText;
    public TextMeshProUGUI saveButtonText;
    public Button saveButton;
    public Button shareButton;

    private Regex regex;
    private AndroidJavaClass versionInfo;
    private Color activeColor;

    private const int androidSSDKVesion = 31;

    private const string initializingMessage = "Tracking is being initialized.";
    private const string relocalizingMessage = "Tracking is resuming after an interruption.";
    private const string insufficientLightMessage = "Too dark. Try moving to a well-lit area.";
    private const string insufficientLightMessageAndroidS = "Too dark. Try moving to a well-lit area. Also, make sure the Block Camera is set to off in system settings.";
    private const string insufficientFeatureMessage = "Can't find anything. Aim device at a surface with more texture or color.";
    private const string excessiveMotionMessage = "Moving too fast. Slow down.";
    private const string unsupportedMessage = "Tracking lost reason is not supported.";

    private void Awake()
    {
        activeColor = saveButtonText.color;
        regex = new Regex("^[a-zA-Z0-9-_]*$");
        versionInfo = new AndroidJavaClass("android.os.Build$VERSION");
    }
    public void WhenEnabled()
    {
        instructionBar.SetActive(true);
        namePanel.SetActive(false);
        inputFieldWarning.SetActive(false);
        shareButton.gameObject.SetActive(false);
    }

    public void OnInputFieldValueChanged(string inputString)
    {
        inputFieldWarning.SetActive(!regex.IsMatch(inputString));
        SetSaveButtonActive(!inputFieldWarning.activeSelf && inputString.Length > 0);
    }
    public void SetInstructionText(string text)
    {
        instructionText.text = text;
    }
    public void SetSaveButtonActive(bool active)
    {
        saveButton.enabled = active;
        saveButtonText.color = active ? activeColor : Color.gray;
    }
    public void SuccessfullHosting(string name)
    {
        nameField.text = name;
        namePanel.SetActive(true);
        SetSaveButtonActive(true);
        Invoke(nameof(DoHideInstructionBar), 1.5f);
    }
    public void DoHideInstructionBar()
    {
        instructionBar.SetActive(false);
    }

    public void SaveButtonClicked()
    {
        shareButton.gameObject.SetActive(true);
        namePanel.SetActive(false);
    }

    public void DisplayTrackingHelperMessage(bool isReturning)
    {
        if (isReturning || ARSession.notTrackingReason == NotTrackingReason.None)
        {
            trackingHelperText.gameObject.SetActive(false);
        }
        else
        {
            trackingHelperText.gameObject.SetActive(true);
            switch (ARSession.notTrackingReason)
            {
                case NotTrackingReason.Initializing:
                    trackingHelperText.text = initializingMessage;
                    return;
                case NotTrackingReason.Relocalizing:
                    trackingHelperText.text = relocalizingMessage;
                    return;
                case NotTrackingReason.InsufficientLight:
                    if (versionInfo.GetStatic<int>("SDK_INT") < androidSSDKVesion)
                    {
                        trackingHelperText.text = insufficientLightMessage;
                    }
                    else
                    {
                        trackingHelperText.text = insufficientLightMessageAndroidS;
                    }

                    return;
                case NotTrackingReason.InsufficientFeatures:
                    trackingHelperText.text = insufficientFeatureMessage;
                    return;
                case NotTrackingReason.ExcessiveMotion:
                    trackingHelperText.text = excessiveMotionMessage;
                    return;
                case NotTrackingReason.Unsupported:
                    trackingHelperText.text = unsupportedMessage;
                    return;
                default:
                    trackingHelperText.text = string.Format("Not tracking reason: {0}", ARSession.notTrackingReason);
                    return;
            }
        }
    }


}
