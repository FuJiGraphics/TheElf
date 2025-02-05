using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum AnimType
{
    Idle, Walk, Attack, Damaged, Die,
};

public class EnemySC : MonoBehaviour, IDefender
{
    public int id = -1;
    public GameObject target;
    public List<GameObject> individualSkills;
    public float activeSkillDuration = 5f;      // 스킬 발동 간격
    public bool isBoss = false;

    public float moveSpeed = 5f;
    public int basicAttack = 10;
    public float attackSpeed = 1f;
    public int healthPoint = 100;
    public int expGained = 0;

    public bool superArmor = false;
    public float stunDuration = 0.1f;

    public float blinkDuration = 0.2f;
    public int blinkCount = 5;
    public float receiveDamageDuration = 0.1f;

    public float deadAnimDuration = 2f;
    public Animator animator;
    public Dictionary<AnimType, Action> animations 
        = new Dictionary<AnimType, Action>();

    public Vector3 right = new Vector3(-1f, 1f, 1f);
    public Vector3 left = Vector3.one;

    public bool isBlockedAttack = false;
    public bool isBlockedMovement = false;

    private List<SpriteRenderer> m_SpriteRenderers;
    private GameObject m_CollidedPlayer;
    private bool m_IsStunned = false;
    private bool m_IsCollided = false;
    private bool m_IsFireIndividualSkills = false;
    private Vector3 m_TargetDir = Vector3.zero;

    private WaitForSeconds m_WaitBlinkDuration;
    private WaitForSeconds m_StunDuration;
    private WaitForSeconds m_DeadAnimDuration;

    private bool m_Attacked = false;

    private Rigidbody2D m_Rigidbody;

    public bool IsDie { get; private set; } = false;

    protected virtual void Start()
    {
        m_SpriteRenderers = new List<SpriteRenderer>(
            GetComponentsInChildren<SpriteRenderer>());
        for (int i = 0; i < m_SpriteRenderers.Count; i++)
        {
            if (m_SpriteRenderers[i].tag == "Shadow")
            {
                m_SpriteRenderers.RemoveAt(i);
            }
        }
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();
        if (m_Rigidbody == null)
        {
            Debug.LogError("Rigidbody�� ã�� �� �����ϴ�.");
        }
        m_WaitBlinkDuration = new WaitForSeconds(blinkDuration);
        m_StunDuration = new WaitForSeconds(stunDuration);
        m_DeadAnimDuration = new WaitForSeconds(deadAnimDuration);
    }

    protected virtual void Update()
    {
        if (!GameManagerSC.Instance.IsPlaying)
            return;

        this.FireIndividualSkill();

        if (!isBlockedMovement && !m_Attacked && !IsDie && !m_IsStunned)
        {
            this.Move();
        }
        if (!isBlockedAttack && m_IsCollided)
        {
            this.Attack();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        bool playerCollided = collision.CompareTag("Player");
        if (collision.CompareTag("Player"))
        {
            m_IsCollided = true;
            m_CollidedPlayer = collision.gameObject;
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = 0f;
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_IsCollided = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_IsCollided = false;
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
        => this.OnTriggerEnter2D(collision.collider);

    protected virtual void OnCollisionStay2D(Collision2D collision)
        => this.OnTriggerStay2D(collision.collider);

    protected virtual void OnCollisionExit2D(Collision2D collision)
        => this.OnTriggerExit2D(collision.collider);

    public void SetMonsterData(MonsterData data)
    {
        this.id = data.Id;
        this.healthPoint = data.HealthPoint;
        this.basicAttack = data.BasicAttack;
        this.expGained = data.ExpGained;
        this.moveSpeed = data.MoveSpeed;
        this.attackSpeed = data.AttackSpeed;
    }

    public virtual void Move()
    {
        var animState = animator.GetCurrentAnimatorStateInfo(0);
        if (!m_IsCollided)
        {
            if (target != null)
            {
                m_TargetDir = target.transform.position - transform.position;
                m_TargetDir.Normalize();
                m_TargetDir.z = 0f;
            }

            Vector3 velocity = m_TargetDir * moveSpeed * Time.deltaTime * 0.01f;
            Vector3 newPosition = transform.position + velocity;
            m_Rigidbody.MovePosition(newPosition);
            if (m_TargetDir != Vector3.zero)
            {
                PlayAnimation(AnimType.Walk);
            }
            transform.localScale = (m_TargetDir.x > 0f) ? right : left;
        }
        else if (animState.normalizedTime >= 1.0f)
        {
            PlayAnimation(AnimType.Idle);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsDie)
            return;

        healthPoint -= damage;
        if (healthPoint < 0)
        {
            healthPoint = 0;
            IsDie = true;
            this.Die();
        }
        else
        {
            PlayAnimation(AnimType.Damaged);
            this.TriggerDamageEffect();
            this.TriggerStunState();
        }
    }

    public void Die()
        => StartCoroutine(DeadCoroutine());

    public void Attack()
        => StartCoroutine(AttackCoroutine());

    public void FireIndividualSkill()
        => StartCoroutine(IndividualSkillCoroutine());

    public void TriggerStunState()
    {
        if (!superArmor)
        {
            StartCoroutine(Stun());
        }
        else
        {
            m_IsStunned = false;
        }
    }

    public void TriggerDamageEffect()
    {
        StartCoroutine(BlinkEffect());
    }

    public void PlayAnimation(AnimType animType)
    {
        if (animations.ContainsKey(animType))
        {
            animations[animType]();
        }
    }

    private IEnumerator BlinkEffect()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            foreach (var renderer in m_SpriteRenderers)
            {
                renderer.color = Color.red;
            }

            yield return m_WaitBlinkDuration;

            foreach (var renderer in m_SpriteRenderers)
            {
                renderer.color = Color.white;
            }

            yield return m_WaitBlinkDuration;
        }
    }

    private IEnumerator Stun()
    {
        m_IsStunned = true;
        yield return m_StunDuration;
        m_IsStunned = false;
    }

    private IEnumerator DeadCoroutine()
    {
        target.GetComponent<ILevelable>().TakeExp(expGained);
        CircleCollider2D collider = GetComponentInParent<CircleCollider2D>();
        if (collider == null)
        {
            collider = GetComponentInChildren<CircleCollider2D>();
        }
        collider.isTrigger = true;
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        if (rb == null)
        {
            rb = GetComponentInChildren<Rigidbody2D>();
        }
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        PlayAnimation(AnimType.Die);
        this.SetSortingOrders(-100);
        yield return m_DeadAnimDuration;
        GameObject.Destroy(gameObject);
    }

    private IEnumerator AttackCoroutine()
    {
        if (!m_Attacked && !IsDie && !m_IsStunned)
        {
            m_Attacked = true;
            PlayAnimation(AnimType.Attack);
            yield return new WaitForSeconds(receiveDamageDuration);
            if (m_CollidedPlayer != null)
            {            
                m_CollidedPlayer.GetComponent<IDefender>().TakeDamage(basicAttack);
            }
            yield return new WaitForSeconds(attackSpeed);
            m_Attacked = false;
        }
    }

    private IEnumerator IndividualSkillCoroutine()
    {
        if (individualSkills.Count > 0 && !m_IsFireIndividualSkills)
        {
            m_IsFireIndividualSkills = true;
            yield return new WaitForSeconds(activeSkillDuration);
            for (int i = 0; i < individualSkills.Count; ++i)
            {
                GameObject skillGo = GameObject.Instantiate(individualSkills[i], transform.position, Quaternion.identity);
                ISkill targetSkill = null;
                if (skillGo != null)
                {
                    targetSkill = skillGo.GetComponent<ISkill>();
                }
                if (targetSkill != null && this.CheckProbability(targetSkill.ActivateProb))
                {
                    if (IsOverlapTag(targetSkill.Collider, "Player"))
                    {
                        isBlockedMovement = true;
                        isBlockedAttack = true;
                        targetSkill.OnFire(gameObject);
                        yield return new WaitForSeconds(targetSkill.SkillDuration);
                        isBlockedMovement = false;
                        isBlockedAttack = false;
                    }
                }
                GameObject.Destroy(skillGo);
            }
            m_IsFireIndividualSkills = false;
        }
    }

    bool IsOverlapTag(Collider2D collider, string tag)
    {
        bool result = false;
        List<Collider2D> hits;
        if (this.FireOverlapCheck(collider, out hits))
        {
            foreach (var hit in hits)
            {
                if (hit.CompareTag(tag))
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    bool FireOverlapCheck(Collider2D collider, out List<Collider2D> hits)
    {
        bool result = false;
        hits = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();

        int hitCount = Physics2D.OverlapCollider(collider, contactFilter, hits);

        if (hitCount > 0)
        {
            result = true;
        }
        return result;
    }

    private void SetSortingOrders(int order)
    {
        for (int i = 0; i < m_SpriteRenderers.Count; ++i)
        {
            m_SpriteRenderers[i].sortingOrder = order;
        }
    }

    bool CheckProbability(float chance)
    {
        float randomValue = UnityEngine.Random.Range(0.0f, 100.0f);
        return randomValue < chance;  // 확률에 맞게 true/false 반환
    }

} // class EnemySC
