using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSC : Singleton<GameManagerSC>
{
    public float timeLimit = 360.0f;
    public GameObject gameUI;

    private GameTimerSC m_TimerUI;
    private bool m_IsPlaying;

    public bool IsPlaying
    {
        get
        {
            if (Time.timeScale < float.Epsilon)
                return false;
            if (!m_TimerUI.IsPlaying)
                return false;
            return m_IsPlaying;
        }
        private set
        {
            m_IsPlaying = value;
        }
    }

    private void Start()
    {
        m_TimerUI = gameUI.GetComponentInChildren<GameTimerSC>();
        if (m_TimerUI == null)
        {
            Debug.Log("TimerUI is null!");
        }

        this.StartGame();
    }

    private void Update()
    {
        if (!this.IsPlaying)
            PauseGame();
        else
            StartGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        IsPlaying = false;
        m_TimerUI.PauseTimer();
    }

    public void RestartGame()
    {
        if (IsPlaying)
            this.PauseGame();

        string currScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currScene);
        this.StartGame();
    }

    public void StartGame()
    {
        if (IsPlaying)
            return;

        Time.timeScale = 1.0f;
        IsPlaying = true;
        m_TimerUI.StopTimer();
        m_TimerUI.StartTimer(timeLimit);
    }

} // class GameManagerSC
