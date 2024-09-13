using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAndDrag : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color pressColor;

    private Color startColor;

    private void Awake()
    {
        startColor = backgroundImage.color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.position.y < rectTransform.sizeDelta.y)
        {
            rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.sizeDelta.y);
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
