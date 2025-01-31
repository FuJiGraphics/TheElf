using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySC : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 5f;
    public int attackPower = 10;
    public float attackSpeed = 1f;
    public int health = 100;

    public bool superArmor = false;
    public float stunDuration = 0.1f;

    public float blinkDuration = 0.2f;
    public int blinkCount = 5;

    private List<SpriteRenderer> spriteRenderers;
    private bool isCollided = false;
    private bool isStunned = false;

    private float m_ElapsedTime = 0f;

    private void Start()
    {
        spriteRenderers = new List<SpriteRenderer>(
            GetComponentsInChildren<SpriteRenderer>());
    }

    private void Update()
    {
        if (!GameManagerSC.Instance.IsPlaying)
            return;

        if (!isCollided && !isStunned)
        {
            this.Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_ElapsedTime = attackSpeed;
            isCollided = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_ElapsedTime += Time.deltaTime;
            if (m_ElapsedTime > attackSpeed)
            {
                m_ElapsedTime = 0f;
                collision.GetComponent<PlayerSC>().Damaged(attackPower);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollided = false;
            m_ElapsedTime = 0f;
        }
    }

    public virtual void Move()
    {
        if (target == null)
            return;

        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.Normalize();
        targetDir.z = 0f;

        Vector3 velocity = targetDir * moveSpeed * Time.deltaTime;
        transform.position += velocity;
    }

    public virtual void OnDamage(int damage)
    {
        health -= damage;
        this.TriggerDamageEffect();
        this.TriggerStunState();
        if (health < 0)
        {
            this.Die();
            health = 0;
        }
    }

    public virtual void Die()
    {
        GameObject.Destroy(gameObject);
    }

    public void TriggerStunState()
    {
        if (!superArmor)
        {
            StartCoroutine(Stun());
        }
        else
        {
            isStunned = false;
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
            foreach (var renderer in spriteRenderers)
            {
                renderer.color = Color.red;
            }

            yield return new WaitForSeconds(blinkDuration);

            foreach (var renderer in spriteRenderers)
            {
                renderer.color = Color.white;
            }

            yield return new WaitForSeconds(blinkDuration);
        }
    }

    private IEnumerator Stun()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

} // class EnemySC
