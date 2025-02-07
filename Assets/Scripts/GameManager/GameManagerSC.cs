using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSC : Singleton<GameManagerSC>
{
    public float timeLimit = 360.0f;
    public GameObject gameUI;

    private GameTimerSC m_TimerUI;
    private DefeatPanelSC m_DefeatUI;
    private VictoryPanelSC m_VictoryUI;
    private ExperienceSC m_ExpUI;
    private EnforcePanelSC m_EnforceUI;
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
        KillCount = 0;

        gameUI = GameObject.Find("GameUI"); 
        LogManager.IsVaild(gameUI);

        m_TimerUI = gameUI.GetComponentInChildren<GameTimerSC>(true);
        LogManager.IsVaild(m_TimerUI);

        m_DefeatUI = gameUI.GetComponentInChildren<DefeatPanelSC>(true);
        LogManager.IsVaild(m_DefeatUI);

        m_VictoryUI = gameUI.GetComponentInChildren<VictoryPanelSC>(true);
        LogManager.IsVaild(m_VictoryUI);

        var expGo = GameObject.FindWithTag("ExperienceUI");
        LogManager.IsVaild(expGo);

        m_ExpUI = expGo.GetComponent<ExperienceSC>();
        LogManager.IsVaild(m_ExpUI);

        m_EnforceUI = gameUI.GetComponentInChildren<EnforcePanelSC>(true);
        LogManager.IsVaild(m_EnforceUI);

        this.SetExp(0, 100);
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

    public void VictoryGame()
    {
        this.PauseGame();
        m_VictoryUI.gameObject.SetActive(true);
    }

    public void EnforceGame(PlayerSC target)
    {
        this.PauseGame();
        m_EnforceUI.SetTargetPlayer(target);
        m_EnforceUI.gameObject.SetActive(true);
    }

    public void AddTimeEvent(int second, Action func)
        => m_TimerUI.AddTimeEvent(second, func);

    public void DeleteTimeEvent(Action func)
        => m_TimerUI.DeleteTimeEvent(func);

    public void SetExp(int value, int maxValue)
        => m_ExpUI.SetExp(value, maxValue);

} // class GameManagerSC
