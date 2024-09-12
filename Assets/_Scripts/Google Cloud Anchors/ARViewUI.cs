using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARViewUI : MonoBehaviour
{
    public GameObject instructionBar;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI trackingHelperText;
    public Button shareButton;

    private AndroidJavaClass versionInfo;

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
        versionInfo = new AndroidJavaClass("android.os.Build$VERSION");
    }
    public void WhenEnabled()
    {
        instructionBar.SetActive(true);
        shareButton.gameObject.SetActive(false);
    }

    public void SetInstructionText(string text)
    {
        instructionText.text = text;
    }

    public void SuccessfullHosting(string name)
    {
        Invoke(nameof(DoHideInstructionBar), 1.5f);
        shareButton.gameObject.SetActive(true);
    }
    public void DoHideInstructionBar()
    {
        instructionBar.SetActive(false);
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
