
public class ItemData : IGameData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public float RecoveryAmountPercent { get; set; }
    public bool GodMode { get; set; }
    public bool DoubleAttackPower { get; set; }
    public bool RemoveAllStatusAbnormalities { get; set; }  // 상태이상 제거
    public bool TimeLimitIncreasedBy30Seconds { get; set; } // 제한 시간 30초 증가
    public int MaximumNumberOfUsesPerStage { get; set; }    // 1 스테이지당 사용 가능한 물약 수
    public int MaximumNumberOfHoldings { get; set; }        // 최대 물약 소지 가능 개수
    public float Duration { get; set; }                     // 지속시간
    public float AppearanceProbability { get; set; }        // 등장 확률

} // class ItemData