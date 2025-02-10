using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceSC : MonoBehaviour
{
    private Slider m_ExpUI;
    private TextMeshProUGUI m_Text;
    private bool m_IsInitialized = false;
    
    private void Awake()
    {
        this.Init();
    }

    public void SetExp(int value, int maxValue)
    {
        this.Init();
        m_ExpUI.minValue = 0;
        m_ExpUI.maxValue = maxValue;
        m_ExpUI.value = value;
        if (value > 0)
        {
            float v = (float)value / maxValue;
            m_Text.text = ((int)(v * 100.0)).ToString() + "%";
        }
        else
        {
            m_Text.text = "0%";
        }
    }

    private void Init()
    {
        if (m_IsInitialized)
            return;
        m_IsInitialized = true;

        m_ExpUI = GetComponentInChildren<Slider>();
        if (m_ExpUI == null)
        {
            Debug.LogError("Did not found Experience Slider!");
        }
        var textUI = GameObject.FindWithTag("RemainExpUI");
        if (textUI == null)
        {
            Debug.LogError("Did not found Experience Text UI Object!");
        }
        m_Text = textUI.GetComponent<TextMeshProUGUI>();
        if (m_Text == null)
        {
            Debug.LogError("Did not found Experience Text UGUI!");
        }
    }

} // class ExperienceSC
