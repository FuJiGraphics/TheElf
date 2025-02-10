
using fz;
using UnityEngine;
using UnityEngine.EventSystems;

public class 
    : MonoBehaviour
    ,IBeginDragHandler, IDragHandler, IEndDragHandler, ITouchData
{
    private RectTransform m_Lever;
    private RectTransform m_RectRransform;

    public int Id { get; private set; } = -1;
    public TouchState State { get; private set; } = TouchState.None;
    public Vector2 Start { get; private set; } = Vector2.zero;
    public Vector2 End { get; private set; } = Vector2.zero;
    public float Distance { get; private set; } = 0.0f;
    public float PressedTime { get; private set; } = 0.0f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 inputPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectRransform, eventData.position, eventData.pressEventCamera, out inputPos);
        m_Lever.anchoredPosition = inputPos;
        State = TouchState.Pressed;
        Start = inputPos.normalized;
        PressedTime += Time.deltaTime;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 inputPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectRransform, eventData.position, eventData.pressEventCamera, out inputPos);
        m_Lever.anchoredPosition = inputPos;
        State = TouchState.Held;
        End = inputPos.normalized;
        PressedTime += Time.deltaTime;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        State = TouchState.None;
        Start = Vector2.zero;
        Distance = 0.0f;
        PressedTime = 0.0f;
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        var rects = GetComponentsInChildren<RectTransform>();
        foreach (var rect in rects)
        {
            switch (rect.name)
            {
                case "Joystick":
                    m_RectRransform = rect;
                    break;
                case "Lever":
                    m_Lever = rect;
                    break;
            }
        }
    }

    private void Update()
    {
        
    }

} // class VirtualJoystick