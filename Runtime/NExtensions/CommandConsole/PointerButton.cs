using UnityEngine;
using UnityEngine.EventSystems;

public class PointerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    public bool Pressed { get; private set; }
    public Vector2 PressOffset { get; private set; }

    [SerializeField] RectTransform rectTransform;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PressOffset = Input.mousePosition - rectTransform.position;
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        PressOffset = Vector2.zero;
    }
}
