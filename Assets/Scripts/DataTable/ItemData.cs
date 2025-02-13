
public class ItemData : IGameData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public float RecoveryAmountPercent { get; set; }
    public bool GodMode { get; set; }
    public bool DoubleAttackPower { get; set; }
    public bool RemoveAllStatusAbnormalities { get; set; }  // �����̻� ����
    public bool TimeLimitIncreasedBy30Seconds { get; set; } // ���� �ð� 30�� ����
    public int MaximumNumberOfUsesPerStage { get; set; }    // 1 ���������� ��� ������ ���� ��
    public int MaximumNumberOfHoldings { get; set; }        // �ִ� ���� ���� ���� ����
    public float Duration { get; set; }                     // ���ӽð�
    public float AppearanceProbability { get; set; }        // ���� Ȯ��

} // class ItemData