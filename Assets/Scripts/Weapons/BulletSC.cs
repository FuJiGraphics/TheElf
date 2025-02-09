using Assets.PixelFantasy.Common.Scripts;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletSC : MonoBehaviour
{
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

    protected virtual void OnDisable()
    {
        m_CurrentHitCount = 0;
        m_ElapsedTime = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var sc = collision.gameObject.GetComponent<EnemySC>();
            sc.TakeDamage(attackPower);
            m_CurrentHitCount++;
            if (sc.IsDie)
            {
                if (sc.isBoss)
                {
                    GameManagerSC.Instance.VictoryGame();
                }
                else
                {
                    // 킬 카운트 증가
                    GameManagerSC.Instance.KillCount++;
                }
            }
            // 맞출 수 있는 적이 초과됐을 경우 현재 오브젝트 반환
            if (m_CurrentHitCount >= maximumTarget)
            {
                this.Destroy();
            }
        }
    }

    protected virtual void MoveTrigger()
    {
        // Empty
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

    public void ImmediateFire()
    {

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

} // class BulletSC
