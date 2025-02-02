
using UnityEngine;

public interface ISkill
{
    public float SkillDuration { get; set; }    // �ߵ� �ð�
    public float ActivateProb { get; set; }     // �ߵ� Ȯ��
    public Collider2D Collider { get; set; }    // �ߵ� ����

    public void OnFire();

} // interface skill
