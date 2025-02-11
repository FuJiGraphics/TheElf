using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSC : Singleton<GameManagerSC>
{
    public int currentStage = 0;
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
    public bool IsNotInGameScene { get; private set; } = true;
    public bool IsSceneLoaded { get; private set; } = false;

    public bool IsPlaying
    {
        get
        {
            if (Time.timeScale < float.Epsilon)
                return false;
            if (m_TimerUI != null && !m_TimerUI.IsPlaying)
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
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Update()
    {
        if (IsNotInGameScene)
            return;

        if (!this.IsPlaying)
        {
            if (m_TimerUI.IsTimeOver)
            {
                this.DefeatGame();
            }
            else
            {
                this.PauseGame();
            }
            return;
        }
        this.StartGame();
    }

    public void Init()
    {
        if (IsNotInGameScene)
            return;

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

    private void LoadDataTables()
    {
        DataTable<PlayerData>.Init("01_Character");
        DataTable<StatData>.Init("02_StatTable");
        DataTable<RandomStatData>.Init("02_RandomStatTable");
        DataTable<LevelData>.Init("03_LevelTable");
        DataTable<MonsterData>.Init("04_MonsterTable");
        DataTable<MonsterSkillData>.Init("05_MonsterSkillTable");
        DataTable<WeaponData>.Init("06_WeaponTable");
        DataTable<WeaponSkillData>.Init("07_WeaponSkillTable");
        DataTable<EnforceData>.Init("08_EnforceTable");
        DataTable<ItemData>.Init("09_ItemTable");
        DataTable<EffectData>.Init("10_EffectTable");
        DataTable<SpawnData>.Init("11_SpawnTable");
        DataTable<NeedExpData>.Init("12_NeedExpTable");
    }

    private void InitWeaponManager()
    {
        WeaponManager.Instance.Init();
    }

    private void Release()
    {
        m_EnforceUI = null;
        m_ExpUI = null;
        m_VictoryUI = null;
        m_DefeatUI = null;
        m_TimerUI = null;
        gameUI = null;
        currentStage = 0;
        timeLimit = 360.0f;
    }

    private void ReleaseDataTables()
    {
        DataTable<PlayerData>.Release();
        DataTable<StatData>.Release();
        DataTable<RandomStatData>.Release();
        DataTable<LevelData>.Release();
        DataTable<MonsterData>.Release();
        DataTable<MonsterSkillData>.Release();
        DataTable<WeaponData>.Release();
        DataTable<WeaponSkillData>.Release();
        DataTable<EnforceData>.Release();
        DataTable<ItemData>.Release();
        DataTable<EffectData>.Release();
        DataTable<SpawnData>.Release();
        DataTable<NeedExpData>.Release();
    }

    private void ReleaseWeaponManager()
    {
        WeaponManager.Instance.Release();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("sceneLoaded Called");
        if (scene.name == "InGameScene")
        {
            this.IsNotInGameScene = false;
            this.LoadDataTables();
            this.InitWeaponManager();
            this.Init();
            this.StartGame();
            this.IsSceneLoaded = true;
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("sceneUnloaded Called");
        if (scene.name == "InGameScene")
        {
            this.IsSceneLoaded = false;
            this.Release();
            this.ReleaseWeaponManager();
            this.ReleaseDataTables();
            this.IsNotInGameScene = true;
        }
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        IsPlaying = false;
        m_TimerUI.PauseTimer();
        VirtualJoystick.Instance?.SetActive(false);
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
        VirtualJoystick.Instance?.SetActive(true);
    }

    public void EndGame()
    {
        SceneManager.LoadScene("LobbyScene");
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
