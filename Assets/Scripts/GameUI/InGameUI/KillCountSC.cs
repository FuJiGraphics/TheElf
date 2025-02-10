using TMPro;
using UnityEngine;

public class KillCountSC : MonoBehaviour
{
    private TextMeshProUGUI m_Text;
    private int m_CurrentKillCount;

    private void Awake()
    {
        m_Text = GetComponentInChildren<TextMeshProUGUI>();
        m_CurrentKillCount = 0;
    }

    private void Update()
    {
        if (!GameManagerSC.Instance.IsPlaying)
            return;

        if (m_CurrentKillCount != GameManagerSC.Instance.KillCount)
        {
            m_CurrentKillCount = GameManagerSC.Instance.KillCount;
            m_Text.text = m_CurrentKillCount.ToString("D4");
        }
    }

} // class KillCountSC
