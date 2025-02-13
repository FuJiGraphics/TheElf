using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : BulletSC
{
    public string attackEffectName;
    public float chainDuration = 0.2f;
    public float chainDistance = 2f;
    public int chainCount = 5;
    public GameObject fireEffect;
    public Quaternion m_FixedRotation = Quaternion.Euler(0f, 0, 90f);
    public float m_FixedPosition = 1f;

    private WaitForSeconds m_WaitDuration;
    private bool m_Running = true;
    private Dictionary<GameObject, bool> m_TargetTable;
    private int m_CurrentCount = 0;

    protected override void InitTrigger(BaseWeapon data)
    {
        this.attackPower = data.attackPower;
        this.maximumTarget = data.monsterMaximumTarget;
        this.chainCount = data.Bounce;
    }

    protected override void EnableTrigger()
    {
        if (m_TargetTable == null)
        {
            m_TargetTable = new Dictionary<GameObject, bool>();
        }
        m_TargetTable.Clear();
        m_WaitDuration = new WaitForSeconds(chainDuration);
        m_Running = true;
        m_CurrentCount = 0;
    }

    protected override void DisableTrigger()
    {
        m_Running = false;
        m_CurrentCount = 0;
    }

    protected override void UpdateTrigger()
    {
        if (!m_Running)
        {
            m_Running = true;
            this.Destroy();
        }
        rb.velocity = Vector3.zero;
    }

    protected override void FireTrigger()
    {
        StartCoroutine(ChainAttackCoroutine());
    }

    protected override MultiAttackInfo MultiAttackTrigger(EnemySC enemy)
    {
        MultiAttackInfo info = new MultiAttackInfo();
        info.stopAttack = true;
        return info;
    }

    private IEnumerator ChainAttackCoroutine()
    {
        GameObject findGo = FindMinDistanceGo(chainDistance);
        while (findGo != null && m_CurrentCount < chainCount)
        {
            EnemySC enemy = findGo?.GetComponent<EnemySC>();
            if (enemy != null && enemy.gameObject.activeInHierarchy && !enemy.IsDie)
            {
                enemy.TakeDamage(base.attackPower);
                this.AdjustScale(findGo.transform);
                m_CurrentCount++;
                yield return m_WaitDuration;
                transform.position = enemy.transform.position;
                findGo = FindMinDistanceGo(chainDistance);
            }
            else
            {
                m_Running = false;
                m_CurrentCount = 0;
                yield break;
            }
        }
        m_Running = false;
        m_CurrentCount = 0;
        yield break;
    }

    private GameObject FindMinDistanceGo(float distance)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, distance);
        GameObject minEnemy = null;
        for (int i = 0; i < hits.Length; ++i)
        {
            GameObject target = hits[i].gameObject;
            if (!target.CompareTag("Enemy"))
            {
                continue;
            }
            if (m_TargetTable.ContainsKey(target))
            {
                continue;
            }
            if (minEnemy == null)
            {
                minEnemy = target;
            }
            else
            {
                var minDir = minEnemy.transform.position - transform.position;
                var tarDir = target.transform.position - transform.position;
                float minDis = minDir.magnitude;
                float tarDis = tarDir.magnitude;
                if (tarDis < minDis)
                {
                    minEnemy = target;
                }
            }
        }
        if (minEnemy != null)
        {
            m_TargetTable.Add(minEnemy, true);
        }
        return minEnemy;
    }

    void AdjustScale(Transform target)
    {
        if (target == null) 
            return;
        transform.rotation = Quaternion.identity;
        float distance = (target.position - transform.position).magnitude;
        transform.localScale = new Vector2(distance, transform.localScale.y);
        var dir = (target.position  - transform.position).normalized;
        transform.position += dir * transform.localScale.x * 0.5f;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir) * m_FixedRotation;
    }

} // class ChainLightning
