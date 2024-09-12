using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolveView : MonoBehaviour
{
    public CloudAnchorController controller;
    public TextMeshProUGUI idText;
    public Button resolveButton;
    public Image resolveButtonImage;

    private Color activeColor;
 

    private void Awake()
    {
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
        }
        else
        {
            SetButtonActive(true);
            idText.text = controller.LoadCurrentCloudAnchorId();
        }


    }


}
