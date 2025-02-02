using Assets.PixelFantasy.Common.Scripts;
using Assets.PixelFantasy.PixelMonsters.Common.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSC : MonoBehaviour, ISkill
{
    public float activateProb = 50f;
    public Collider2D activeBounds = null;
    public int accelerateCount = 8;
    public int lossTargetTimingIndex = 2;
    public float accelarateDuration = 1f;
    public float increaseInSpeed = 50f;

    public float SkillDuration { get; set; } = 3f;
    public float ActivateProb { get; set; } = 50f;
    public Collider2D Collider { get; set; } = null;

    private GameObject m_OriginTarget = null;
    private EnemySC m_CurrentOwner = null;
    private float m_originMoveSpeed = 0f;

    private void Awake()
    {
        SkillDuration = (accelerateCount * accelarateDuration) 
            + (accelerateCount * 0.5f * accelarateDuration) + 1f;
        ActivateProb = activateProb;
        Collider = activeBounds;
    }

    public void OnFire(GameObject owner)
    {
        m_CurrentOwner = owner.GetComponent<EnemySC>();
        m_CurrentOwner.isBlockedMovement = false; // 스킬을 써도 움직일 수 있게 함
        m_CurrentOwner.isBlockedAttack = false;
        this.Charge();
    }

    private void Charge()
        => StartCoroutine(ChargeCoroutine());

    private IEnumerator ChargeCoroutine()
    {
        m_OriginTarget = m_CurrentOwner.target;
        m_originMoveSpeed = m_CurrentOwner.moveSpeed;
        for (int i = 0; i < accelerateCount; ++i)
        {
            if (lossTargetTimingIndex == i)
            {
                m_CurrentOwner.target = null;
            }
            m_CurrentOwner.moveSpeed += this.increaseInSpeed;
            EffectManager.Instance.CreateSpriteEffect(m_CurrentOwner.gameObject, "Run", Vector2.one * 3f);
            yield return new WaitForSeconds(accelarateDuration);
        }
        for (int i = 0; i < accelerateCount; ++i)
        {
            m_CurrentOwner.moveSpeed -= this.increaseInSpeed;
            EffectManager.Instance.CreateSpriteEffect(m_CurrentOwner.gameObject, "Brake", Vector2.one * 3f);
            yield return new WaitForSeconds(accelarateDuration * 0.5f);
        }
        m_CurrentOwner.moveSpeed = m_originMoveSpeed;
        m_CurrentOwner.target = m_OriginTarget;
    }

} // class ChargeSC
