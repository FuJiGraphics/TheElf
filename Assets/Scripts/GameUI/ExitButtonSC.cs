using UnityEngine;
using UnityEngine.UI;

public class ExitButtonSC : MonoBehaviour
{
    public GameObject activeObject;

    public int disableSecond = 120;
    private Button m_ExitButton;
    private Image[] m_Images;

    private void Awake()
    {
        m_ExitButton = GetComponent<Button>();
        m_Images = GetComponentsInChildren<Image>();
        m_ExitButton.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        GameManagerSC.Instance.AddTimeEvent(disableSecond, Disable);
    }

    private void OnEnable()
    {
        for (int i = 0; i < m_Images.Length; ++i)
        {
            m_Images[i].color = Color.white;
        }
    }

    public void OnClick()
    {
        if (!GameManagerSC.Instance.IsPlaying)
        {
            return;
        }

        if (activeObject == null)
        {
            Debug.LogError("active object를 찾을 수 없습니다.");
            return;
        }
        activeObject.SetActive(true);
    }

    public void Disable()
    {
        m_ExitButton.enabled = false;
        for (int i = 0; i < m_Images.Length; ++i)
        {
            m_Images[i].color = new Color(0f, 0f, 0f, 0f);
        }
    }

} // class ExitButtonSC
