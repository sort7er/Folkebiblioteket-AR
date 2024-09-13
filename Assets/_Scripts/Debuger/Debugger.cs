using TMPro;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private TextMeshProUGUI messagePrefab;
    [SerializeField] private TextMeshProUGUI descriptionPrefab;

    private void Awake()
    {
        background.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}
