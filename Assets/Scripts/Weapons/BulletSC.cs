using System.Collections;
using UnityEngine;

public class BulletSC : MonoBehaviour
{
    public class MultiAttackInfo
    {
        public int attackCount = 0;
        public float attackDuration = 0f;
        public bool stopAttack = false;
    }

    public int attackPower = 20;
    public float survivalTime = 5f;
    public float projectileSpeed = 10f;
    public Vector3 direction = Vector3.forward;
    public int maximumTarget = 10;
    public GameObject effect;
    public ObjectManagerSC ownerPool;

    private float m_ElapsedTime = 0f;
    private SpriteRenderer m_SpriteRenderer;
    private int m_CurrentHitCount = 0;

    protected Quaternion firstRotation;
    protected Rigidbody2D rb;
    protected GameObject owner;
    protected Vector3 setPosition = Vector3.zero;
    protected Vector3 setDirection = Vector3.zero;
    protected Quaternion setRotation = Quaternion.identity;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.enabled = false;
        firstRotation = transform.rotation;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;
        this.MoveTrigger();
        if (m_ElapsedTime >= survivalTime)
        {
            this.Destroy();
            m_ElapsedTime = 0f;
        }
    }

    private void OnEnable()
    {
        EnableTrigger();
    }

    private void OnDisable()
    {
        m_CurrentHitCount = 0;
        m_ElapsedTime = 0f;
        StopAllCoroutines();
        DisableTrigger();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf)
            return;

        if (collision.CompareTag("Enemy"))
        {
            var sc = collision.gameObject.GetComponent<EnemySC>();
            if (sc != null && !sc.IsDie)
            {
                MultiAttackInfo info = MultiAttackTrigger(sc);
                if (info != null && info.stopAttack)
                {
                    return;
                }
                else if (info == null)
                {
                    AttackEnemy(sc);
                }
                else
                {
                    AttackEnemy(sc, info.attackCount, info.attackDuration);
                }
            }
        }
    }

    protected virtual void EnableTrigger()
    {
        // Empty
    }

    protected virtual void DisableTrigger()
    {
        // Empty
    }

    protected virtual void MoveTrigger()
    {
        // Empty
    }

    protected virtual MultiAttackInfo MultiAttackTrigger(EnemySC enemy)
    {
        return null;
    }

    protected void Destroy()
    {
        if (ownerPool)
        {
            ownerPool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetWeaponData(BaseWeapon data)
    {
        this.attackPower = data.attackPower;
        this.maximumTarget = data.monsterMaximumTarget;
    }

    public void Fire(Vector3 position, Vector3 direction, int maximumTarget, 
        float projectileSpeed = 10f, float survivalTime = 5f)
    {
        this.maximumTarget = maximumTarget;
        this.projectileSpeed = projectileSpeed;
        this.survivalTime = survivalTime;
        this.Fire(position, direction, attackPower);
    }

    public void Fire(Vector3 position, Vector3 direction, Quaternion rotation, GameObject owner)
    {
        m_SpriteRenderer.enabled = true;
        this.transform.rotation = firstRotation * rotation;
        rb.velocity = direction * projectileSpeed;
        transform.position = position;
        m_ElapsedTime = 0f;
        var particle = GetComponentInChildren<ParticleSystem>();
        if (particle != null)
        {
            particle.transform.rotation = this.transform.rotation;
        }
        this.setPosition = position;
        this.setDirection = direction;
        this.setRotation = rotation;
        this.owner = owner;
    }


    private void AttackEnemy(EnemySC enemy, int count = 1, float duration = 0)
    {
        if (enemy == null)
            return;

        if (count == 1 && duration == 0)
        {
            this.Attack(enemy);
        }
        else if (enemy != null)
        {
            GameManagerSC.Instance.StartCoroutine(this.AttackEnemyCoroutine(enemy, count, duration));
        }
        m_CurrentHitCount++;
        if (m_CurrentHitCount >= maximumTarget)
        {
            this.Destroy();
        }
    }

    private IEnumerator AttackEnemyCoroutine(EnemySC enemy, int count, float duration)
    {
        if (enemy == null || !enemy.gameObject.activeInHierarchy)
        {
            yield break;
        }

        for (int i = 0; i < count; ++i)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy) 
                yield break;
            this.Attack(enemy);
            yield return new WaitForSeconds(duration);
        }
    }

    void Attack(EnemySC enemy)
    {
        enemy.TakeDamage(attackPower);
        if (enemy.IsDie)
        {
            if (enemy.isBoss)
            {
                GameManagerSC.Instance.VictoryGame();
            }
            else
            {
                GameManagerSC.Instance.KillCount++;
            }
        }
    }

    protected void ResetSuvivalTime()
    => m_ElapsedTime = 0f;

} // class BulletSC
