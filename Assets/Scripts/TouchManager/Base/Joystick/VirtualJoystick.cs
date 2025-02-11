
using fz;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick: MonoBehaviour
    ,IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform m_Lever;
    private RectTransform m_BoundsRect;
    private RectTransform m_RectTransform;
    private CircleCollider2D m_CircleBounds;

    public bool Active { get; private set; } = false;
    public Vector2 Direction { get; private set; } = Vector2.zero;
    public float Distance { get; private set; } = 0f;
    public float PressedTime { get; private set; } = 0f;

    // Statics
    public static GameObject Instance { get; set; }

    private void Awake()
    {
        this.Init();
    }

    private void Init()
    {
        Instance = this.gameObject;
        m_CircleBounds = GetComponentInChildren<CircleCollider2D>();
        var rects = GetComponentsInChildren<RectTransform>();
        foreach (var rect in rects)
        {
            switch (rect.name)
            {
                case "Joystick":
                    m_BoundsRect = rect;
                    break;
                case "Layout":
                    m_RectTransform = rect;
                    break;
                case "Lever":
                    m_Lever = rect;
                    break;
            }
        }
    }

    private void Clear()
    {
        Active = false;
        Direction = Vector2.zero;
        PressedTime = 0.0f;
        m_Lever.anchoredPosition = Vector2.zero;
        m_RectTransform.gameObject.SetActive(false);
        m_Lever.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_RectTransform.gameObject.SetActive(true);
        m_Lever.gameObject.SetActive(true);
        m_RectTransform.position = eventData.position;
        m_Lever.anchoredPosition = Vector2.zero;
        Active = true;
        PressedTime = 0f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 inputPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform, eventData.position, eventData.pressEventCamera, out inputPos);
        m_Lever.anchoredPosition = inputPos;
        PressedTime += Time.deltaTime;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPos = Vector2.zero;
        Vector2 inputPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform, eventData.position, eventData.pressEventCamera, out inputPos);
        float mag = inputPos.magnitude;
        if (mag > m_CircleBounds.radius)
        {
            newPos = inputPos.normalized * m_CircleBounds.radius;
        }
        else
        {
            newPos = inputPos;
        }

        Direction = newPos.normalized;
        Distance = newPos.magnitude / m_CircleBounds.radius;
        m_Lever.anchoredPosition = newPos;
        PressedTime += Time.deltaTime;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.Clear();
    }

} // class VirtualJoystick