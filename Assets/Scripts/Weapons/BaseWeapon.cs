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
    public float Duration = 0;
    public int Count = 0;
    public int Bounce = 0;
    public float RotationSpeed = 0f;
    public float DamagePerSecond = 0f;
    public List<int> basicAttackRange;

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
    }

} // class PlayerWeaponSC
