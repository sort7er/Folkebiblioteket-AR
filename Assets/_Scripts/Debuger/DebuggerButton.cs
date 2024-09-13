using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebuggerButton : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color pressColor;

    private Color startColor;
    private float edge;

    private void Awake()
    {
        startColor = backgroundImage.color;
        edge = Screen.height - rectTransform.sizeDelta.y;
    }

    public void OnDrag(PointerEventData eventData)
    {     

        if(eventData.position.y > edge)
        {
            rectTransform.position = new Vector2(rectTransform.position.x, edge);
        }
        else
        {
            rectTransform.position = new Vector2(rectTransform.position.x, eventData.position.y);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundImage.color = pressColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        backgroundImage.color = startColor;
    }
}
