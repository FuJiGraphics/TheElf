using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SignUpState
{
    Done,
    ExistsID,
    LeastID,
    EmptyID,
    LeastPassword,
    EmptyPassword,
    EmptyUsername,
    UnexpectedError,
};

public class GameManagerSC : Singleton<GameManagerSC>
{
    public readonly string guestId = "aoi@fk!j%qw$d9&14%jd01*)(*^osdpkfe";
    public readonly string guestPassword = "349592mf#@$@$@FDsd@!!~CA";
    public readonly string saveFileName = "SaveData";

    public int currentStage = 0;
    public float timeLimit = 600f;
    public GameObject gameUI;

    private GameTimerSC m_TimerUI;
    private DefeatPanelSC m_DefeatUI;
    private VictoryPanelSC m_VictoryUI;
    private ExperienceSC m_ExpUI;
    private EnforcePanelSC m_EnforceUI;
    private bool m_IsPlaying;
    private FadeInOut m_FadeInOutController;

    private EnemyFindBounds m_EnemyFindBounds;

    public bool FadeOutDone { get; set; } = true;
    public float CurrentTime { get => m_TimerUI.ElapsedTime; }
    public int KillCount { get; set; }
    public bool IsNotInGameScene { get; private set; } = true;
    public bool IsSceneLoaded { get; private set; } = false;
    public GameObject RandomEnemyTarget { get => m_EnemyFindBounds?.CurrentEnemy; }
    public SaveData GameData { get; private set; }

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
            else if (FadeOutDone)
            {
                this.PauseGame();
            }
            return;
        }
        this.StartGame();
    }

    public bool LoginId(string id, string password)
    {
        bool result = false;
        if (id == guestId && password == guestPassword)
        {
            SaveData data = new SaveData();
            data.Id = guestId;
            data.Name = "Guest";
            data.Password = guestPassword;
            data.MaxHealthPoint = 0;
            data.AttackSpeed = 0;
            data.ClearStage = 0;
            data.MoveSpeed = 0;
            data.Coin = 1000;
            this.SignUp(data);
            result = true;
        }
        this.LoadSaveData();
        if (DataTable<SaveData>.Exists(id))
        {
            SaveData tempUserData = DataTable<SaveData>.At(id);
            if (tempUserData.Password == password)
            {
                GameData = DataTable<SaveData>.At(id);
                result = true;
            }
        }
        return result;
    }

    public SignUpState SignUp(SaveData data)
    {
        if (data == null)
        {
            return SignUpState.UnexpectedError;
        }
        if (data.Id.Length == 0)
        {
            return SignUpState.EmptyID;
        }
        if (data.Id.Length < 6)
        {
            return SignUpState.LeastID;
        }
        if (data.Password.Length == 0)
        {
            return SignUpState.EmptyPassword;
        }
        if (data.Password.Length < 6)
        {
            return SignUpState.LeastPassword;
        }
        if (data.Name.Length == 0)
        {
            return SignUpState.EmptyUsername;
        }
        this.LoadSaveData();
        if (DataTable<SaveData>.Exists(data.Id))
        {
            return SignUpState.ExistsID;
        }
        DataTable<SaveData>.SaveFromPersistentData(saveFileName, data);
        return SignUpState.Done;
    }

    public void FadeIn()
    {
        StartCoroutine(this.FadeInCoroutine());
    }

    public IEnumerator FadeInCoroutine()
    {
        GameObject fadeGo = UtilManager.FindWithName("FadeInOut");
        if (m_FadeInOutController == null)
        {
            m_FadeInOutController = fadeGo?.GetComponent<FadeInOut>();
        }
        m_FadeInOutController?.FadeIn();
        yield return new WaitForSeconds(m_FadeInOutController.playTime);
        fadeGo.SetActive(false);
    }

    public void FadeOut(Action trigger)
    {
        StartCoroutine(this.FadeOutCoroutine(trigger));
    }

    public IEnumerator FadeOutCoroutine(Action trigger)
    {
        FadeOutDone = false;
        GameObject fadeGo = UtilManager.FindWithName("FadeInOut");
        if (m_FadeInOutController == null)
        {
            m_FadeInOutController = fadeGo?.GetComponent<FadeInOut>();
        }
        m_FadeInOutController?.gameObject.SetActive(true);
        m_FadeInOutController?.FadeOut();
        yield return new WaitForSeconds(m_FadeInOutController.playTime);
        FadeOutDone = true;
        trigger.Invoke();
    }

    public void Init()
    {
        if (IsNotInGameScene)
            return;

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

        var boundsGo = GameObject.FindWithTag("EnemyFindBounds");
        LogManager.IsVaild(boundsGo);

        m_EnemyFindBounds = boundsGo.GetComponent<EnemyFindBounds>();
        LogManager.IsVaild(m_EnemyFindBounds);

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

    private void LoadSaveData()
    {
        DataTable<SaveData>.InitFromPersistentData(saveFileName);
    }

    public void ClearSaveData()
    {
        CsvManager.RemoveInPersistentDataPath<SaveData>(saveFileName);
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

    private void SaveGameData()
    {
        if (GameData != null)
        {
            // Test Play
            Debug.Log("Active Test Play");
            CsvManager.SaveInPersistentDataPath<SaveData>(GameData, "SaveData.csv");
        }
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
            this.LoadSaveData();
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
            this.SaveGameData();
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
        GameManagerSC.Instance.FadeOut(() => LoadSceneManager.LoadScene(currScene));
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
        this.StartGame();
        GameManagerSC.Instance.FadeOut(() => LoadSceneManager.LoadScene("LobbyScene"));
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
