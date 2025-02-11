using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : BulletSC
{
    [SerializeField] GameObject projectileEffect;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] float projectileRadius;
    [SerializeField] float explosionRadius;
    [SerializeField] float damagePerSecond;
    [SerializeField] bool singleAttackMode = false;

    private bool m_IsExploded = false;
    private float m_ExplosionElapsedTime = 0f;
    private WaitForSeconds m_WaitDamagePerSecond;

    protected override void EnableTrigger()
    {
        projectileEffect?.SetActive(true);
        explosionEffect?.SetActive(false);
        GetComponent<CircleCollider2D>().radius = projectileRadius;
        m_ExplosionElapsedTime = 0f;
        m_IsExploded = false;
        m_WaitDamagePerSecond = new WaitForSeconds(damagePerSecond);
    }

    protected override void DisableTrigger()
    {
        m_ExplosionElapsedTime = 0f;
        m_IsExploded = false;
    }

    protected override MultiAttackInfo MultiAttackTrigger(EnemySC enemy)
    {
        MultiAttackInfo info = new MultiAttackInfo();
        info.stopAttack = true;
        this.Explode();
        return info;
    }

    private void Explode()
    {
        if (m_IsExploded)
            return;
        m_IsExploded = true;
        if (singleAttackMode)
        {
            this.ExplodeSingleAttack();
        }
        else
        {
            GameManagerSC.Instance.StartCoroutine(this.ExplodeCoroutine());
        }
    }

    private IEnumerator ExplodeCoroutine()
    {
        m_IsExploded = true;
        rb.velocity = Vector3.zero;
        this.ResetSuvivalTime();
        projectileEffect?.SetActive(false);
        explosionEffect?.SetActive(true);

        while (m_ExplosionElapsedTime <= base.survivalTime)
        {
            m_ExplosionElapsedTime += Time.deltaTime;
            this.ExplodeAttack();
            yield return m_WaitDamagePerSecond;
        }
    }

    private void ExplodeSingleAttack()
    {
        m_IsExploded = true;
        rb.velocity = Vector3.zero;
        projectileEffect?.SetActive(false);
        explosionEffect?.SetActive(true);
        this.ExplodeAttack();
    }

    private void ExplodeAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        for (int i = 0; i < hits.Length; ++i)
        {
            GameObject target = hits[i].gameObject;
            EnemySC enemy = target?.GetComponent<EnemySC>();
            if (enemy != null && !enemy.IsDie)
            {
                enemy.TakeDamage(base.attackPower);
            }
        }
    }

} // class Explosion