using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;
using UnityEngine.UIElements;

public class PlayerSC : MonoBehaviour
    , IDefender, ILevelable, IEquipment
{
    public int id = 1;
    public float moveSpeed;
    public int healthPoint;
    public float skillCoolDown;
    public List<int> statKeyIds;
    public List<int> randomStatKeyIds;
    public List<int> effectKeyIds;
    public List<int> expKeyIds;

    private TouchManager m_Touch;
    private Vector3 m_CurrDir;
    private Vector3 m_TargetPos;
    private bool m_HasMoved;

    private Animator m_Animator;

    private HealthBarSC m_HealthBar;
    private int m_CurrHealth;
    private int m_CurrExp;

    private Rigidbody2D m_Rigidbody;

    public int CurrentExp { get; set; }
    public bool IsDie { get; private set; } = false;
    public int Level { get; set; } = 1;

    public Longbow Longbow { get; set; }
    public Crossbow Crossbow { get; set; }
    public Sword Sword { get; set; }
    private List<BaseWeapon> Weapons { get; set; }

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
            this.TouchMove();
            this.Attack();
        }
    }

    public void TakeDamage(int damage)
    {
        m_CurrHealth = Mathf.Clamp(m_CurrHealth - damage, 0, healthPoint);
        m_HealthBar.SetHealth(m_CurrHealth);
        if (m_CurrHealth == 0)
        {
            IsDie = true;
            StartCoroutine(CoroutineDead());
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
        }
        else
        {
            m_CurrExp = nextExp;
        }
        GameManagerSC.Instance.SetExp(m_CurrExp, maxExp);
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
        IsDie = false;
    }

    private void LoadData()
    {
        DataTable<PlayerData>.Init("01_Character");
        PlayerData playerData = DataTable<PlayerData>.Get(id);
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
            expKeyIds.Add(DataTable<NeedExpData>.Get(id).NeedExp);
        }
    }

    private void InitStartingWeapon()
    {
        Weapons = new List<BaseWeapon>();
        BaseWeapon ranFirstWeapon = this.SetRandomWeapon();
        Weapons.Add(ranFirstWeapon);
    }

    private BaseWeapon SetRandomWeapon()
    {
        BaseWeapon ret = null;
        int index = UnityEngine.Random.Range(0, 2);
        switch (index)
        {
            case 0: 
                Longbow = new Longbow();
                ret = Longbow;
                break;
            case 1: 
                Crossbow = new Crossbow();
                ret = Crossbow;
                break;
            case 2: 
                Sword = new Sword();
                ret = Sword;
                break;
        }
        return ret;
    }

    private void TouchMove()
    {
        if (m_Touch.TouchHeld.Active)
        {
            m_TargetPos = Camera.main.ScreenToWorldPoint(m_Touch.TouchHeld.Position);
            m_TargetPos.z = 0f;
            m_CurrDir = (m_TargetPos - transform.position).normalized;
            m_CurrDir.z = 0f;
            m_HasMoved = true;
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
            m_HasMoved = false;
        }
        m_Rigidbody.MovePosition(newPosition);

        m_Animator.SetFloat("x", m_CurrDir.x);
        m_Animator.SetFloat("y", m_CurrDir.y);
        m_Animator.SetBool("Walk", m_HasMoved);
    }

    private void Attack()
    {
        for (int i = 0; i < Weapons.Count; ++i)
        {
            Weapons[i].Shoot(m_CurrDir, transform.position);
        }
    } 

    private IEnumerator CoroutineDead()
    {
        m_Animator.SetBool("Dead", true);
        var animState = m_Animator.GetCurrentAnimatorStateInfo(0);
        while (animState.normalizedTime < 1.0f)
        {
            yield return null;
        }
        GameManagerSC.Instance.DefeatGame();
    }

} // class PlayerMovement
