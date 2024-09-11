using TMPro;
using UnityEngine;

public class Debuger : MonoBehaviour
{
    public static Debuger Instance;



    [SerializeField] private TextMeshProUGUI textPrefab;
    [SerializeField] private TextMeshProUGUI textTallPrefab;
    [SerializeField] private Transform textContainer;
    [SerializeField] private bool includeStack;

    private int limit = 10;

    private TextMeshProUGUI[] textElements;
    private TextMeshProUGUI[] tallTextElements;

    private int textIdx = 0;
    private int tallTextIdx = 0;

    private void Awake()
    {
        Instance = this;
        Application.logMessageReceived += ConsolePrint;
        InstatntiateText();
    }

    private void InstatntiateText()
    {
        textElements = new TextMeshProUGUI[limit];
        tallTextElements = new TextMeshProUGUI[limit];

        for (int i = 0; i < limit; i++)
        {
            textElements[i] = Instantiate(textPrefab, textContainer);
            tallTextElements[i] = Instantiate(textTallPrefab, textContainer);

            textElements[i].gameObject.SetActive(false);
            tallTextElements[i].gameObject.SetActive(false);
        }
    }

    public void DebugMessage(string message)
    {
        Debug.Log(message);
    }
    public void DebugError(string message)
    {
        Debug.LogError(message);
    }
    public void ConsolePrint(string logString, string stackTrace, LogType type)
    {
        SetTextPrefab(GetNextText(), logString, type);

        if(includeStack)
        {
            SetTextPrefab(GetNextTallText(), stackTrace, type);
        }

    }

    private void SetTextPrefab(TextMeshProUGUI prefab, string log, LogType type)
    {
        prefab.gameObject.SetActive(true);
        prefab.text = log;
        prefab.transform.SetParent(null);
        prefab.transform.SetParent(textContainer);

        if (type == LogType.Error)
        {
            prefab.color = Color.red;
        }
        else if (type == LogType.Warning)
        {
            prefab.color = Color.yellow;
        }
    }


    private TextMeshProUGUI GetNextText()
    {
        if(textIdx < textElements.Length)
        {
            return textElements[textIdx++];
        }
        else
        {
            return textElements[textIdx = 0];
        }
    }
    private TextMeshProUGUI GetNextTallText()
    {
        if (tallTextIdx < tallTextElements.Length)
        {
            return tallTextElements[tallTextIdx++];
        }
        else
        {
            return tallTextElements[tallTextIdx = 0];
        }
    }

}
