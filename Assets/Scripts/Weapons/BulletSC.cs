using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSC : MonoBehaviour
{
    public int attackPower = 20;
    public float survivalTime = 5f;
    public float projectileSpeed = 10f;
    public Vector3 direction = Vector3.forward;
    public int maximumTarget = 10;
    public ObjectManagerSC ownerPool;

    private float m_ElapsedTime = 0f;
    private SpriteRenderer m_SpriteRenderer;
    private Quaternion m_FirstRotation;
    private int m_CurrentHitCount = 0;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.enabled = false;
        m_FirstRotation = transform.rotation;
    }

    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;
        if (m_ElapsedTime >= survivalTime)
        {
            this.Destroy();
            m_ElapsedTime = 0f;
        }
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

    private void Destroy()
    {
        m_CurrentHitCount = 0;
        m_SpriteRenderer.enabled = false;
        gameObject.SetActive(false);
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

    public void Fire(Vector3 position, Vector3 direction, Quaternion rotation)
    {
        m_SpriteRenderer.enabled = true;
        this.transform.rotation = m_FirstRotation * rotation;
        GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        transform.position = position;
        m_ElapsedTime = 0f;
    }

} // class BulletSC
