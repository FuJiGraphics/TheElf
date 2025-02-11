using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSC : MonoBehaviour
    , IDefender, ILevelable
{
    public enum MovementType
    {
        Touch, Joystick
    };

    public int id = 1;
    public float moveSpeed;
    public int healthPoint;
    public float skillCoolDown;
    public MovementType movementType = MovementType.Joystick;
    public GameObject joystick = null;

    // IEquipment
    public List<GameObject> allWeapons;

    public List<int> statKeyIds;
    public List<int> randomStatKeyIds;
    public List<int> effectKeyIds;
    public List<int> expKeyIds;

    private TouchManager m_Touch;
    private Vector3 m_CurrDir;
    private Vector3 m_TargetPos;

    public float blinkDuration = 0.2f;
    public int blinkCount = 5;

    private SpriteRenderer m_SpriteRenderer;
    private WaitForSeconds m_WaitBlinkDuration;

    private Animator m_Animator;

    private HealthBarSC m_HealthBar;
    private int m_CurrHealth;
    private int m_CurrExp;

    private Rigidbody2D m_Rigidbody;

    public int CurrentExp { get; set; }
    public bool IsDie { get; private set; } = false;
    public int Level { get; set; } = 1;

    private List<WeaponSC> m_EquippedWeapons;
    private VirtualJoystick m_VirtualJoystick;

    private void Start()
    {
        this.Init();
        this.LoadData();
        this.InitStartingWeapon();
    }

    private void FixedUpdate()
    {
        if (!GameManagerSC.Instance.IsPlaying)
            return;

        if (!IsDie)
        { 
            this.Movement();
            this.Attack();
        }
    }

    public void TakeDamage(int damage)
    {
        m_CurrHealth = Mathf.Clamp(m_CurrHealth - damage, 0, healthPoint);
        m_HealthBar.SetHealth(m_CurrHealth);
        this.TriggerDamageEffect();
        if (m_CurrHealth == 0)
        {
            IsDie = true;
            StartCoroutine(CoroutineDead());
        }
    }

    public void TriggerDamageEffect()
    {
        StartCoroutine(BlinkEffect());
    }

    private IEnumerator BlinkEffect()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            m_SpriteRenderer.color = Color.red;
            yield return m_WaitBlinkDuration;

            m_SpriteRenderer.color = Color.white;
            yield return m_WaitBlinkDuration;
        }
    }

    public void TakeExp(int exp)
    {
        if (Level >= expKeyIds.Count)
            return;

        DataTable<NeedExpData>.Init("12_NeedExpTable");

        int maxExp = expKeyIds[Level - 1];
        int nextExp = m_CurrExp + exp;
        if (nextExp >= maxExp)
        {
            m_CurrExp = 0;
            Level++;
            GameManagerSC.Instance.EnforceGame(this);
        }
        else
        {
            m_CurrExp = nextExp;
        }
        GameManagerSC.Instance.SetExp(m_CurrExp, maxExp);
    }

    public void ChangeItem(GameObject from, GameObject to)
    {
        bool isFind = false;
        for (int i = 0; i < allWeapons.Count; ++i)
        {
            if (allWeapons[i] == from)
            {
                isFind = true;
                allWeapons[i] = to;
                break;
            }
        }
        if (isFind)
        {
            m_EquippedWeapons = new List<WeaponSC>();
            foreach (var go in allWeapons)
            {
                WeaponSC sc = go.GetComponent<WeaponSC>();
                m_EquippedWeapons.Add(sc);
            }
        }
    }

    public List<WeaponSC> GetUnusedWeapons()
    {
        List<WeaponSC> unused = null;
        for (int i = 0; i < allWeapons.Count; ++i)
        {
            if (allWeapons[i].activeSelf == false)
            {
                if (unused == null)
                {
                    unused = new List<WeaponSC>();
                }
                WeaponSC sc = allWeapons[i].GetComponent<WeaponSC>();
                if (LogManager.IsVaild(sc))
                {
                    unused.Add(sc);
                }
            }
        }
        return unused;
    }

    private void Init()
    {
        m_Touch = TouchManager.Instance;
        m_CurrDir = Vector2.right;
        m_Animator = GetComponent<Animator>();
        m_HealthBar = GetComponentInChildren<HealthBarSC>();
        m_HealthBar.SetMaxHealth(healthPoint);
        m_CurrHealth = healthPoint;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_WaitBlinkDuration = new WaitForSeconds(blinkDuration);
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        joystick = GameObject.FindWithTag("fz_Joystick");
        if (joystick != null)
        {
            m_VirtualJoystick = joystick.GetComponent<VirtualJoystick>();
        }
        IsDie = false;
    }

    private void LoadData()
    {
        DataTable<PlayerData>.Init("01_Character");
        PlayerData playerData = DataTable<PlayerData>.At(id);
        this.id = playerData.Id;
        this.name = playerData.Name;
        this.skillCoolDown = playerData.SkillCoolDown;
        this.healthPoint = playerData.HealthPoint;
        this.moveSpeed = playerData.MoveSpeed;
        this.statKeyIds = CsvManager.ToList<int>(playerData.StatKeyIds);
        this.randomStatKeyIds = CsvManager.ToList<int>(playerData.RandomStatKeyIds);
        this.effectKeyIds = CsvManager.ToList<int>(playerData.EffectKeyIds);

        // 필요 경험치 로드
        var expList = CsvManager.ToList<int>(playerData.ExpKeyIds);
        DataTable<NeedExpData>.Init("12_NeedExpTable");
        expKeyIds = new List<int>();
        for (int i = 0; i < expList.Count; ++i)
        {
            int id = expList[i];
            expKeyIds.Add(DataTable<NeedExpData>.At(id).NeedExp);
        }
    }

    private void InitStartingWeapon()
    {
        if (allWeapons == null || allWeapons.Count < 1)
        {
            Debug.Log("Empty Weapons!");
            return;
        }
        for (int i = 0; i < allWeapons.Count; ++i)
        {
            allWeapons[i].SetActive(false);
        }
        int ran = Random.Range(0, allWeapons.Count);
        allWeapons[4].SetActive(true);
        m_EquippedWeapons = new List<WeaponSC>();
        foreach (var weapon in allWeapons)
        {
            WeaponSC sc = weapon.GetComponent<WeaponSC>();
            m_EquippedWeapons.Add(sc);
        }
    }

    private void Movement()
    {
        switch (movementType)
        {
            case MovementType.Touch:
                this.TouchMove();
                break;
            case MovementType.Joystick:
                this.JoystickMove();
                break;
        }
    }

    private void TouchMove()
    {
        if (m_Touch.TouchHeld.Active)
        {
            m_TargetPos = Camera.main.ScreenToWorldPoint(m_Touch.TouchHeld.Position);
            m_TargetPos.z = 0f;
            m_CurrDir = (m_TargetPos - transform.position).normalized;
            m_CurrDir.z = 0f;
            m_Animator.SetFloat("x", m_CurrDir.x);
            m_Animator.SetFloat("y", m_CurrDir.y);
            m_Animator.SetBool("Walk", true);
        }
        else
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Animator.SetBool("Walk", false);
            return;
        }

        Vector3 velocity = (m_CurrDir * moveSpeed * Time.deltaTime) * 0.01f;
        float currMag = (m_TargetPos - transform.position).magnitude;
        Vector3 newPosition;
        if (currMag > velocity.magnitude)
        {
            newPosition = transform.position + velocity;
        }
        else
        {
            newPosition = m_TargetPos;
        }
        m_Rigidbody.MovePosition(newPosition);
    }

    private void JoystickMove()
    {
        if (m_VirtualJoystick == null)
        {
            if (LogManager.IsVaild(joystick))
            {
                m_VirtualJoystick = joystick.GetComponent<VirtualJoystick>();
            }
        }

        if (m_VirtualJoystick.Active)
        {
            if (m_VirtualJoystick.Direction.magnitude > float.Epsilon)
            {
                m_CurrDir = m_VirtualJoystick.Direction;
            }
            m_CurrDir.z = 0f;
            m_Animator.SetFloat("x", m_CurrDir.x);
            m_Animator.SetFloat("y", m_CurrDir.y);
            m_Animator.SetBool("Walk", true);
        }
        else
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Animator.SetBool("Walk", false);
            return;
        }

        Vector3 velocity = (m_CurrDir * moveSpeed * Time.deltaTime) * 0.01f;
        Vector3 newPosition = transform.position + (velocity * m_VirtualJoystick.Distance);
        m_Rigidbody.MovePosition(newPosition);
    }

    private void Attack()
    {
        for (int i = 0; i < m_EquippedWeapons.Count; ++i)
        {
            if (m_EquippedWeapons[i] != null && m_EquippedWeapons[i].gameObject.activeSelf)
            {
                var look = Quaternion.LookRotation(Vector3.forward, m_CurrDir);
                m_EquippedWeapons[i].Fire(m_CurrDir, transform.position, look);
            }
        }
    } 

    private IEnumerator CoroutineDead()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        m_Animator.SetTrigger("Dead");
        var animState = m_Animator.GetCurrentAnimatorStateInfo(0);
        while (animState.normalizedTime < 1.0f)
        {
            yield return null;
        }
        GameManagerSC.Instance.DefeatGame();
    }

} // class PlayerMovement
