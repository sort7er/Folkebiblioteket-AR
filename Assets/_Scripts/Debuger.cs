using TMPro;
using UnityEngine;

public class Debuger : MonoBehaviour
{
    public static Debuger Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private TextMeshProUGUI textPrefab;
    [SerializeField] private Transform textContainer;

    public void DebugMessage(string message)
    {
        TextMeshProUGUI prefab = Instantiate(textPrefab, textContainer);
        prefab.text = message;
    }
    public void DebugError(string message)
    {
        TextMeshProUGUI prefab = Instantiate(textPrefab, textContainer);
        prefab.text = message;
        prefab.color = Color.red;
    }


}
