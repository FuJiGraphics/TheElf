using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class GameTimerSC : MonoBehaviour
{
    public bool IsPlaying { get; private set; } = false;

    public TextMeshProUGUI m_TimerText;
    private WaitForSeconds m_Wait;
    private float m_TimeLimit;
    private float m_ElapsedTime;
    private int m_Minute;
    private int m_Second;
    private bool m_Paused;

    private readonly float m_TimeWeight = 60f;

    private void Start()
    {
        m_TimerText = GetComponent<TextMeshProUGUI>();
        m_Wait = new WaitForSeconds(1f);
        m_Paused = false;
        this.ResetTimer();
    }

    public void StartTimer(float limitSecond)
    {
        if (m_Paused)
        {
            m_Paused = false;
            return;
        }
        m_TimeLimit = limitSecond;
        StopCoroutine(CoroutineTimer());
        StartCoroutine(CoroutineTimer());
    }

    public void StopTimer()
    {
        this.ResetTimer();
        StopCoroutine(CoroutineTimer());
    }

    public void PauseTimer()
        => m_Paused = true;

    public void ResetTimer()
    {
        m_Paused = false;
        m_ElapsedTime = m_TimeLimit;
        this.AdjustTimer();
    }

    private IEnumerator CoroutineTimer()
    {
        this.ResetTimer();
        IsPlaying = true;
        while (m_ElapsedTime > 0)
        {
            yield return m_Wait;
            if (!m_Paused)
            {
                m_ElapsedTime -= 1f;
                this.AdjustTimer();
            }
        }
        IsPlaying = false;
    }

    private void AdjustTimer()
    {
        m_Minute = (int)(m_ElapsedTime / m_TimeWeight);
        m_Second = (int)(m_ElapsedTime % m_TimeWeight);
        string format = m_Minute.ToString() + ":" + m_Second.ToString("D2");
        m_TimerText.text = format;
    }

} // class GameTimerSC
