using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform container;
    [SerializeField] private DebugPrefab debugPrefab;
    [SerializeField] private Image pauseImage;
    [SerializeField] private Sprite pause, play;

    private bool isPaused = false;


    private DebugPrefab[] debugPrefabs;
    private int limit = 60;
    private int debugIdx = 0;

    //private float timer;
    //private int count = 0;
    private void Awake()
    {
        background.sizeDelta = new Vector2(Screen.width, Screen.height);

        Application.logMessageReceived += ConsolePrint;
        InstatntiateText();
    }

    //private void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer > 1)
    //    {
    //        timer = 0;
    //        count++;
    //        Debug.Log(count);

    //        if (count % 5 == 0)
    //        {
    //            Debug.LogWarning("No");
    //        }
    //        if (count % 10 == 0)
    //        {
    //            Debug.LogError("Hmm");
    //        }
    //    }
    //}

    private void InstatntiateText()
    {
        debugPrefabs = new DebugPrefab[limit];

        for (int i = 0; i < limit; i++)
        {
            debugPrefabs[i] = Instantiate(debugPrefab, container);

            debugPrefabs[i].gameObject.SetActive(false);
        }
    }
    public void ConsolePrint(string logString, string stackTrace, LogType type)
    {
        if (!isPaused)
        {
            GetNextDebug().SetUpLog(logString, stackTrace, type, container);
        }
    }

    private DebugPrefab GetNextDebug()
    {
        if (debugIdx < debugPrefabs.Length)
        {
            return debugPrefabs[debugIdx++];
        }
        else
        {
            return debugPrefabs[debugIdx = 0];
        }
    }

    public void PressPause()
    {
        if (isPaused)
        {
            pauseImage.sprite = pause;
            isPaused = false;
        }
        else
        {
            pauseImage.sprite = play;
            isPaused = true;
        }
    }
}
