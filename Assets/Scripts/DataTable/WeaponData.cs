
using System.Collections.Generic;

public class WeaponData : IGameData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BasicAttack { get; set; }
    public float BasicAttackSpeed { get; set; }
    public string BasicAttackRange { get; set; }
    public int MonsterMaximumTarget { get; set; }
    public string EffectKeyIds { get; set; }
    public string SkillKeyIds { get; set; }

} // class WeaponData