using TMPro;
using UnityEngine;

public class DebugPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private RectTransform rectTransform;

    private Vector2 smallSize;
    private Vector2 largeSize;

    private void Awake()
    {
        smallSize = new Vector2(Screen.width, 50);
        largeSize = new Vector2(Screen.width, 300);
    }


    public void SetUpLog(string logString, string stackTrace, LogType type, Transform container)
    {
        gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(false);

        rectTransform.sizeDelta = smallSize;

        transform.SetParent(null);
        transform.SetParent(container);

        text.text = logString;
        descriptionText.text = stackTrace;


        if (type == LogType.Error)
        {
            text.color = Color.red;
            descriptionText.color = Color.red;
        }
        else if (type == LogType.Warning)
        {
            text.color = Color.yellow;
            descriptionText.color = Color.yellow;
        }
        else if (type == LogType.Log)
        {
            text.color = Color.white;
            descriptionText.color = Color.white;
        }
    }

    public void ShowDescription()
    {
        if(!descriptionText.gameObject.activeSelf)
        {
            descriptionText.gameObject.SetActive(true);
            rectTransform.sizeDelta = largeSize;
        }
        else
        {
            descriptionText.gameObject.SetActive(false);
            rectTransform.sizeDelta = smallSize;
        }
    }
}
