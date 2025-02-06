using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSC : MonoBehaviour
{
    public Sprite sprite;
    public int attackPower = 20;
    public float survivalTime = 5f;
    public float projectileSpeed = 10f;
    public Vector3 direction = Vector3.forward;
    public int maximumTarget = 10;
    public ObjectManagerSC ownerPool;

    private float m_ElapsedTime = 0f;
    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = sprite;
        m_SpriteRenderer.enabled = false;
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
        }
    }

    private void Destroy()
    {
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

    public void ImmediateFire()
    {

    }

    public void Fire(Vector3 position, Vector3 direction, int attackPower)
    {
        m_SpriteRenderer.sprite = sprite;
        m_SpriteRenderer.enabled = true;
        this.attackPower = attackPower;
        GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        transform.position = position;
        m_ElapsedTime = 0f;
    }

    public void Fire(Vector3 position, Vector3 direction, int attackPower,
        int maximumTarget, float projectileSpeed = 10f, float survivalTime = 5f)
    {
        this.maximumTarget = maximumTarget;
        this.projectileSpeed = projectileSpeed;
        this.survivalTime = survivalTime;
        this.Fire(position, direction, attackPower);
    }

} // class BulletSC
