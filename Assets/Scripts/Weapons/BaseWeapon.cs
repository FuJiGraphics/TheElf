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
    public int monsterMaximumTarget = 10;
    public List<int> basicAttackRange;
    public List<int> effectKeyIds;
    public List<int> skillKeyIds;

    public WeaponType WeaponType { get; set; }
    public int WeaponLevel { get; set; } = 0;
    public void DeepCopy(ref BaseWeapon other)
    {
        this.id = other.id;
        this.itemName = other.itemName;
        this.attackPower = other.attackPower;
        this.attackSpeed = other.attackSpeed;
        this.monsterMaximumTarget = other.monsterMaximumTarget;
        this.WeaponType = other.WeaponType;
        this.WeaponLevel = other.WeaponLevel;
        UtilManager.Copy(out this.basicAttackRange, ref other.basicAttackRange);
        UtilManager.Copy(out this.effectKeyIds, ref other.effectKeyIds);
        UtilManager.Copy(out this.skillKeyIds, ref other.skillKeyIds);
    }

} // class PlayerWeaponSC
