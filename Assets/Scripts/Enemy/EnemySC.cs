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
    public ISkill individualSkills;
    public ISkill collectiveSkills;

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

    private List<SpriteRenderer> m_SpriteRenderers;
    private GameObject m_CollidedPlayer;
    private bool m_IsStunned = false;
    private bool m_IsDead = false;
    private bool m_IsCollided = false;

    private bool m_Attacked = false;

    private Rigidbody2D m_Rigidbody;

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
    }

    protected virtual void Update()
    {
        if (!GameManagerSC.Instance.IsPlaying)
            return;

        if (!m_Attacked && !m_IsDead && !m_IsStunned)
        {
            this.Move();
        }
        if (m_IsCollided)
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
        if (target == null)
            return;

        var animState = animator.GetCurrentAnimatorStateInfo(0);
        if (!m_IsCollided)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            targetDir.Normalize();
            targetDir.z = 0f;

            Vector3 velocity = targetDir * moveSpeed * Time.deltaTime * 0.01f;
            Vector3 newPosition = transform.position + velocity;
            m_Rigidbody.MovePosition(newPosition);
            PlayAnimation(AnimType.Walk);
            transform.localScale = (targetDir.x > 0f) ? right : left;
        }
        else if (animState.normalizedTime >= 1.0f)
        {
            PlayAnimation(AnimType.Idle);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (m_IsDead)
            return;

        healthPoint -= damage;
        if (healthPoint < 0)
        {
            healthPoint = 0;
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

    private IEnumerator BlinkEffect()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            foreach (var renderer in m_SpriteRenderers)
            {
                renderer.color = Color.red;
            }

            yield return new WaitForSeconds(blinkDuration);

            foreach (var renderer in m_SpriteRenderers)
            {
                renderer.color = Color.white;
            }

            yield return new WaitForSeconds(blinkDuration);
        }
    }

    private IEnumerator Stun()
    {
        m_IsStunned = true;
        yield return new WaitForSeconds(stunDuration);
        m_IsStunned = false;
    }

    private IEnumerator DeadCoroutine()
    {
        m_IsDead = true;
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
        yield return new WaitForSeconds(deadAnimDuration);
        GameObject.Destroy(gameObject);
    }

    private IEnumerator AttackCoroutine()
    {
        if (!m_Attacked && !m_IsDead && !m_IsStunned)
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

    private void PlayAnimation(AnimType animType)
    {
        if (animations.ContainsKey(animType))
        {
            animations[animType]();
        }
    }

    private void SetSortingOrders(int order)
    {
        for (int i = 0; i < m_SpriteRenderers.Count; ++i)
        {
            m_SpriteRenderers[i].sortingOrder = order;
        }
    }

} // class EnemySC
