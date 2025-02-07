using System;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponManager
{
    private static readonly string s_RandomStatFilename = "02_RandomStatTable";
    private static readonly string s_WeaponDataFilename = "06_WeaponTable";
    private static readonly string s_LevelDataFilename = "03_LevelTable";
    private static readonly int s_LongbowId = 3011;
    private static readonly int s_CrossbowId = 3012;
    private static readonly int s_SwordId = 3013;

    private static List<int> s_WeaponIds;
    private static List<Dictionary<int, BaseWeapon>> s_Weapons;

    public static BaseWeapon GetInfo(WeaponType type, int level)
    {
        WeaponManager.Init();
        BaseWeapon result = null;
        int weaponType = (int)type;
        if (weaponType < s_Weapons.Count && s_Weapons[weaponType].ContainsKey(level))
        {
            var weapon = s_Weapons[weaponType][level];
            result = new BaseWeapon();
            result.DeepCopy(ref weapon);
        }
        return result;
    }

    public static GameObject LevelUp(WeaponSC weapon)
    {
        WeaponManager.Init();
        int nextLevel = weapon.info.WeaponLevel + 1;
        BaseWeapon target = null;
        int targetWeapon = (int)weapon.info.WeaponType;
        if (s_Weapons[targetWeapon].ContainsKey(nextLevel))
        {
            target = new BaseWeapon();
            BaseWeapon src = s_Weapons[targetWeapon][nextLevel];
            target.DeepCopy(ref src);
        }
        return UtilManager.FindWithName(target.itemName);
    }

    private static void Init()
    {
        if (s_Weapons != null)
        {
            bool hasNull = false;
            for (int i = 0; i < s_Weapons.Count; ++i)
            {
                hasNull = s_Weapons[i] == null;
                if (hasNull)
                    break;
            }
            if (!hasNull)
            {
                return;
            }
        }

        DataTable<RandomStatData>.Init(s_RandomStatFilename);
        DataTable<WeaponData>.Init(s_WeaponDataFilename);
        DataTable<LevelData>.Init(s_LevelDataFilename);

        int size = (int)WeaponType.Max;
        s_WeaponIds = new List<int>(size)
        {
            s_LongbowId, s_CrossbowId, s_SwordId
        };
        s_Weapons = new List<Dictionary<int, BaseWeapon>>(size);
        for (int i = 0; i < size; ++i)
        {
            s_Weapons.Add(new Dictionary<int, BaseWeapon>());
            WeaponManager.LoadTechData((WeaponType)i);
        }
    }

    private static void LoadTechData(WeaponType type)
    {
        var data = DataTable<RandomStatData>.At(s_WeaponIds[(int)type]);
        WeaponManager.AttachWeapon(type, data);
    }

    private static void AttachWeapon(WeaponType type, RandomStatData ranStatData)
    {
        List<int> itemList = CsvManager.ToList<int>(ranStatData.ItemPayments);
        List<int> levelIds = CsvManager.ToList<int>(ranStatData.LevelKeyIds);
        if (itemList.Count != levelIds.Count)
        {
            // 아이템 개수와 레벨의 개수가 대응되지 않음
            throw new Exception();
        }
        for (int i = 0; i < itemList.Count; ++i)
        {
            int id = itemList[i];
            LevelData levelData = DataTable<LevelData>.At(levelIds[i]);
            var weaponData = DataTable<WeaponData>.At(id);
            BaseWeapon baseWeapon = new BaseWeapon();
            baseWeapon.id = weaponData.Id;
            baseWeapon.itemName = weaponData.Name;
            baseWeapon.attackPower = weaponData.BasicAttack;
            baseWeapon.attackSpeed = weaponData.BasicAttackSpeed;
            baseWeapon.basicAttackRange = CsvManager.ToList<int>(weaponData.BasicAttackRange);
            baseWeapon.monsterMaximumTarget = weaponData.MonsterMaximumTarget;
            baseWeapon.effectKeyIds = CsvManager.ToList<int>(weaponData.EffectKeyIds);
            baseWeapon.skillKeyIds = CsvManager.ToList<int>(weaponData.SkillKeyIds);
            baseWeapon.WeaponLevel = levelData.Level;
            baseWeapon.WeaponType = type;
            s_Weapons[(int)type].Add(levelData.Level, baseWeapon);
        }
    }

} // static class LevelManager