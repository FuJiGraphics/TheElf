
using System.Collections.Generic;

public class WeaponData : IGameData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BasicAttack { get; set; }
    public float BasicAttackSpeed { get; set; }
    public string BasicAttackRange { get; set; }
    public int MonsterMaximumTarget { get; set; }
    public float Duration { get; set; }
    public int Count { get; set; }
    public int Bounce { get; set; }
    public float RotationSpeed { get; set; }
    public float DamagePerSecond { get; set; }

} // class WeaponData