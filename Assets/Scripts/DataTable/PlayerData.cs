using CsvHelper.Configuration;
using System.Collections.Generic;

public class PlayerData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float SkillCoolDown { get; set; }
    public int HealthPoint { get; set; }
    public float MoveSpeed { get; set; }
    public string KeyIds { get; set; }
}