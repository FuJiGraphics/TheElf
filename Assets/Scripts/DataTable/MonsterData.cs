
public class MonsterData : IGameData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int HealthPoint { get; set; }
    public int BasicAttack { get; set; }
    public int ExpGained { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackSpeed { get; set; }
    public string SkillKeyIds { get; set; }
    public string EffectKeyIds { get; set; }
} // class MonsterData