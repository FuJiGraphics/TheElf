
public enum WeaponType
{
    Default,
    Longbow,
    Crossbow,
    Sword
}

public interface IWeaponLevel
{
    public WeaponType WeaponType { get; set; }
    public int WeaponLevel { get; set; }
}