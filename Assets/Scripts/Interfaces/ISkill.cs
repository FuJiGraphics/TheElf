
using UnityEngine;

public interface ISkill
{
    public float SkillDuration { get; set; }    // 발동 지속 시간
    public float ActivateProb { get; set; }     // 발동 확률
    public Collider2D Collider { get; set; }    // 발동 범위

    public void OnFire(GameObject owner);

} // interface skill
