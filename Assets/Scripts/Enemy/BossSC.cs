using Assets.PixelFantasy.PixelMonsters.Common.Scripts;
using Assets.PixelFantasy.PixelMonsters.Common.Scripts.ExampleScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSC : EnemySC
{
    private Animator m_Animator;
    private MonsterAnimation m_Animation;

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
        => this.OnTriggerEnter2D(collision.collider);

    protected override void OnCollisionExit2D(Collision2D collision)
        => this.OnTriggerExit2D(collision.collider);

    private void Init()
    {
        this.SetMonsterData(EnemyTable.Get(id));
        m_Animator = GetComponentInChildren<Animator>();
        base.animator = m_Animator;
        m_Animation = GetComponentInChildren<MonsterAnimation>();
        base.right = Vector3.one;
        base.left = new Vector3(-1f, 1f, 1f);
        base.isBoss = true;
    }

    private void AttackAnimations()
    {
        animations.Add(AnimType.Idle,
            () => { m_Animation.SetState(MonsterState.Idle); });
        animations.Add(AnimType.Walk,
            () => { m_Animation.SetState(MonsterState.Walk); });
        animations.Add(AnimType.Attack,
            () => { m_Animation.Attack(); });
        animations.Add(AnimType.Damaged,
            () => { m_Animation.Hit(); });
        animations.Add(AnimType.Die,
            () => { m_Animation.SetState(MonsterState.Die); });
    }
} // class BossSC
