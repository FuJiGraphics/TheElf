using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSC : Singleton<GameManagerSC>
{
    public float timeLimit = 360.0f;
    public GameObject gameUI;

    private GameTimerSC m_TimerUI;
    private DefeatPanelSC m_DefeatUI;
    private bool m_IsPlaying;

    public float CurrentTime { get => m_TimerUI.ElapsedTime; }
    public int KillCount { get; set; }

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

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        this.Init();
        this.StartGame();
    }

    private void Update()
    {
        if (!this.IsPlaying)
        {
            this.PauseGame();
            return;
        }
        this.StartGame();
    }

    public void Init()
    {
        timeLimit = 360.0f;
        m_IsPlaying = false;

        gameUI = GameObject.Find("GameUI");
        if (gameUI == null)
        {
            Debug.Log("Did not found Game UI!");

        }
        m_TimerUI = gameUI.GetComponentInChildren<GameTimerSC>(true);
        if (m_TimerUI == null)
        {
            Debug.Log("Did not found Timer UI!");
        }
        m_DefeatUI = gameUI.GetComponentInChildren<DefeatPanelSC>(true);
        if (m_DefeatUI == null)
        {
            Debug.Log("Did not found Defeat UI!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("sceneLoaded Called");
        if (scene.name == "InGameScene")
        {
            this.Init();
            this.StartGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        IsPlaying = false;
        m_TimerUI.PauseTimer();
    }

    public void RestartGame()
    {
        m_TimerUI.StopTimer();
        this.StartGame();
        string currScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currScene);
    }

    public void StartGame()
    {
        if (IsPlaying)
            return;

        Time.timeScale = 1.0f;
        IsPlaying = true;
        m_TimerUI.StartTimer(timeLimit);
    }

    public void DefeatGame()
    {
        this.PauseGame();
        m_DefeatUI.gameObject.SetActive(true);
    }

    public void AddTimeEvent(int second, Action func)
        => m_TimerUI.AddTimeEvent(second, func);

    public void DeleteTimeEvent(Action func)
        => m_TimerUI.DeleteTimeEvent(func);

} // class GameManagerSC
