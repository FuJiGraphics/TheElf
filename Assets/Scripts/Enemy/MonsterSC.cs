using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSC : EnemySC
{
    private SPUM_Prefabs m_Prefabs;

    protected override void Start()
    {
        base.Start();
        this.Init();
        this.AttackAnimations();
    }

    protected override void Update()
        => base.Update();

    protected override void OnTriggerEnter2D(Collider2D collision)
        => base.OnTriggerEnter2D(collision);

    protected override void OnTriggerExit2D(Collider2D collision)
        => base.OnTriggerExit2D(collision);

    protected override void OnCollisionEnter2D(Collision2D collision)
        => base.OnCollisionEnter2D(collision);

    protected override void OnCollisionExit2D(Collision2D collision)
        => base.OnCollisionExit2D(collision);

    private void Init()
    {
        this.SetMonsterData(EnemyTable.Get(id));
        m_Prefabs = GetComponentInChildren<SPUM_Prefabs>();
        if (m_Prefabs == null)
        {
            Debug.LogError("Prefab의 Script를 찾을 수 없습니다.");
        }
        m_Prefabs.OverrideControllerInit();
        base.animator = m_Prefabs.GetComponentInChildren<Animator>();
        base.right = new Vector3(-1f, 1f, 1f);
        base.left = Vector3.one;
        base.isBoss = false;
    }

    private void AttackAnimations()
    {
        animations.Add(AnimType.Idle, 
            () => { m_Prefabs.PlayAnimation(PlayerState.IDLE, 0); } );
        animations.Add(AnimType.Walk, 
            () => { m_Prefabs.PlayAnimation(PlayerState.MOVE, 0); });
        animations.Add(AnimType.Attack, 
            () => { m_Prefabs.PlayAnimation(PlayerState.ATTACK, 0); });
        animations.Add(AnimType.Damaged, 
            () => { m_Prefabs.PlayAnimation(PlayerState.DAMAGED, 0); });
        animations.Add(AnimType.Die, 
            () => { m_Prefabs.PlayAnimation(PlayerState.DEATH, 0); });
    }

} // class MonsterSC
