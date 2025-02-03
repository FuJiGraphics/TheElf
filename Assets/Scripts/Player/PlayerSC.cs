using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSC : MonoBehaviour, IDefender
{
    public int id = -1;
    public float moveSpeed;
    public int healthPoint;
    public float skillCoolDown;
    public List<int> keyIds;
    public WeaponSC[] weapons;

    private TouchManager m_Touch;
    private Vector3 m_CurrDir;
    private Vector3 m_TargetPos;
    private bool m_HasMoved;

    private Animator m_Animator;

    private HealthBarSC m_HealthBar;
    private int m_CurrHealth;

    private Rigidbody2D m_Rigidbody;

    public bool IsDie { get; private set; } = false;
    
    private void Start()
    {
        m_Touch = TouchManager.Instance;
        m_CurrDir = Vector2.right;
        m_Animator = GetComponent<Animator>();
        m_HealthBar = GetComponentInChildren<HealthBarSC>();
        m_HealthBar.SetMaxHealth(healthPoint);
        m_CurrHealth = healthPoint;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        this.LoadData();
        IsDie = false;
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

    private void LoadData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("01_Character");
        if (csvFile == null)
        {
            Debug.LogError("CSV ������ ã�� �� �����ϴ�.");
        }
        var records = CsvManager.LoadFromText<PlayerData>(csvFile.text);
        PlayerData playerData = records.First();
        this.id = playerData.Id;
        this.moveSpeed = playerData.MoveSpeed;
        this.healthPoint = playerData.HealthPoint;
        this.skillCoolDown = playerData.SkillCoolDown;
        // keyIds = CsvManager.ToList<int>(playerData.KeyIds);
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
        for (int i = 0; i < weapons.Length; ++i)
        {
            weapons[i].Shoot(m_CurrDir, transform.position);
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
