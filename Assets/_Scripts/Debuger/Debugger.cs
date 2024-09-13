using TMPro;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform container;
    [SerializeField] private TextMeshProUGUI messagePrefab;
    [SerializeField] private TextMeshProUGUI descriptionPrefab;


    private TextMeshProUGUI[] messageElements;
    private TextMeshProUGUI[] descriptionTextElements;
    private int limit = 30;
    private int messageIdx = 0;
    private int descriptionIdx = 0;

    private float timer;
    private int yes;

    private void Awake()
    {
        background.sizeDelta = new Vector2(Screen.width, Screen.height);

        Application.logMessageReceived += ConsolePrint;
        InstatntiateText();
    }

    private void InstatntiateText()
    {
        messageElements = new TextMeshProUGUI[limit];
        descriptionTextElements = new TextMeshProUGUI[limit];

        for (int i = 0; i < limit; i++)
        {
            messageElements[i] = Instantiate(messagePrefab, container);
            descriptionTextElements[i] = Instantiate(descriptionPrefab, container);

            messageElements[i].gameObject.SetActive(false);
            descriptionTextElements[i].gameObject.SetActive(false);
        }
    }
    public void ConsolePrint(string logString, string stackTrace, LogType type)
    {
        if (type != LogType.Log)
        {
            SetPrint(GetNextDescription(), stackTrace, type);
        }
        SetPrint(GetNextMessage(), logString, type);
    }

    private void SetPrint(TextMeshProUGUI prefab, string log, LogType type)
    {
        prefab.gameObject.SetActive(true);
        prefab.text = log;
        prefab.transform.SetParent(null);
        prefab.transform.SetParent(container);

        if (type == LogType.Error)
        {
            prefab.color = Color.red;
        }
        else if (type == LogType.Warning)
        {
            prefab.color = Color.yellow;
        }
        else if (type == LogType.Log)
        {
            prefab.color = Color.white;
        }
    }


    private TextMeshProUGUI GetNextMessage()
    {
        if (messageIdx < messageElements.Length)
        {
            return messageElements[messageIdx++];
        }
        else
        {
            return messageElements[messageIdx = 0];
        }
    }
    private TextMeshProUGUI GetNextDescription()
    {
        if (descriptionIdx < descriptionTextElements.Length)
        {
            return descriptionTextElements[descriptionIdx++];
        }
        else
        {
            return descriptionTextElements[descriptionIdx = 0];
        }
    }
}
