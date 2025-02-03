using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class GameTimerSC : MonoBehaviour
{
    public bool IsPlaying { get; private set; } = false;
    public float ElapsedTime { get => Mathf.Clamp(m_TimeLimit - m_Duration, 0f, m_TimeLimit); }

    public TextMeshProUGUI m_TimerText;
    private WaitForSeconds m_Wait;
    private float m_TimeLimit;
    private float m_Duration;
    private int m_Minute;
    private int m_Second;
    private bool m_Paused;
    private Dictionary<int, List<Action>> m_TimeEvents;

    private readonly float m_TimeWeight = 60f;

    private void Awake()
    {
        m_TimerText = GetComponent<TextMeshProUGUI>();
        m_Wait = new WaitForSeconds(1f);
        m_Paused = false;
        m_TimeEvents = new Dictionary<int, List<Action>>();
        this.ResetTimer();
    }

    private void OnEnable()
    {
        ResetTimer();
        IsPlaying = false;
    }

    private void OnDisable()
    {
        StopCoroutine(CoroutineTimer());
        m_Paused = false;
        m_TimeEvents.Clear();
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
        m_Duration = m_TimeLimit;
        this.AdjustTimer();
    }

    public void AddTimeEvent(int second, Action func)
    {
        if (!m_TimeEvents.ContainsKey(second))
        {
            m_TimeEvents.Add(second, new List<Action>());
        }
        if (!m_TimeEvents[second].Contains(func))
        {
            m_TimeEvents[second].Add(func);
        }
    }

    public void DeleteTimeEvent(Action func)
    {
        foreach (var events in m_TimeEvents)
        {
            if (events.Value.Contains(func))
            {
                events.Value.Remove(func);
            }
        }
    }

    private IEnumerator CoroutineTimer()
    {
        this.ResetTimer();
        IsPlaying = true;
        while (m_Duration > 0)
        {
            yield return m_Wait;
            if (!m_Paused)
            {
                m_Duration -= 1f;
                this.AdjustTimer();
                this.OnTimeEvents();
            }
        }
        IsPlaying = false;
    }

    private void AdjustTimer()
    {
        m_Minute = (int)(m_Duration / m_TimeWeight);
        m_Second = (int)(m_Duration % m_TimeWeight);
        string format = m_Minute.ToString("D2") + ":" + m_Second.ToString("D2");
        m_TimerText.text = format;
    }

    private void OnTimeEvents()
    {
        int second = (int)ElapsedTime;
        if (m_TimeEvents.ContainsKey(second))
        {
            for (int i = 0; i < m_TimeEvents[second].Count; ++i)
            {
                m_TimeEvents[second][i]();
            }
        }
    }

} // class GameTimerSC
