using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pressed;

    public Image backgroundImage;
    public Color pressedColor;

    public UnityEvent onPressStart;
    public UnityEvent onPress;
    public UnityEvent onPressEnd;

    private Color startColor;

    private void Awake()
    {
        startColor = backgroundImage.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        onPressStart?.Invoke();
        backgroundImage.color = pressedColor;

    }


    private void Update()
    {
        if (pressed)
        {
            onPress?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        onPressEnd?.Invoke();
        backgroundImage.color = startColor;
    }
}
