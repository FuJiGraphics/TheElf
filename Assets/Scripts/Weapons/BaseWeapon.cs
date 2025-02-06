using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Longbow,
    Crossbow,
    Sword,
    Max,
};

public class BaseWeapon : IWeaponLevel
{
    public int id = -1;
    public string itemName = "Empty";
    public int attackPower = 1;
    public float attackSpeed = 1f;
    public List<int> basicAttackRange;
    public int monsterMaximumTarget = 10;
    public List<int> effectKeyIds;
    public List<int> skillKeyIds;

    public WeaponType WeaponType { get; set; }
    public int WeaponLevel { get; set; } = 0;

} // class PlayerWeaponSC
