
public class PlayerData : IGameData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public float SkillCoolDown { get; set; }
    public int HealthPoint { get; set; }
    public float MoveSpeed { get; set; }
    public string StatKeyIds { get; set; }
    public string RandomStatKeyIds { get; set; }
    public string EffectKeyIds { get; set; }
    public string ExpKeyIds { get; set; }

} // class PlayerData