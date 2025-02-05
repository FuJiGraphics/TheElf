
public class SpawnData : IGameData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float SpawnCycle { get; set; }
    public int MaxCount { get; set; }
    public float Offset { get; set; }
    public float Duration { get; set; }
    public string MonsterKeyIds { get; set; }

} // class EffectData