
using System.Collections.Generic;

public class StatData : IGameData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NeedStatPoints { get; set; }
    public string SkillCoolDownReductions { get; set; }
    public string MaxHealthUpList { get; set; }
    public string SpeedUpList { get; set; }
    public string ItemPayments { get; set; }
    public string LevelKeyIds { get; set; }
} // class StatData