
using System;
using System.Collections.Generic;

public static class WeaponManager
{
    public enum TableType
    {
        RandomStatTable,
        WeaponTable,
        LevelTable,
        Max,
    }

    private static List<string> s_TableFilenames;
    private static List<int> s_EnforceTableIds;
    private static List<Dictionary<int, BaseWeapon>> s_Weapons;
    private static bool s_IsInitialized = false;

    public static BaseWeapon Get(WeaponType type, int level)
    {
        WeaponManager.Init();
        BaseWeapon data = null;
        int weaponType = (int)type;
        if (s_Weapons[weaponType].ContainsKey(level))
        {
            var weapon = s_Weapons[weaponType][level];
            data = WeaponManager.DeepCopy(ref weapon);
        }
        return data;
    }

    public static BaseWeapon LevelUp(ref BaseWeapon target)
    {
        WeaponManager.Init();
        int nextLevel = target.WeaponLevel + 1;
        int weaponType = (int)target.WeaponType;
        BaseWeapon nextWeapon = s_Weapons[weaponType][nextLevel];
        return WeaponManager.DeepCopy(ref nextWeapon);
    }

    private static void Init()
    {
        if (s_IsInitialized)
            return;
        s_IsInitialized = true;
        int size = (int)WeaponType.Max;

        // Init Weapon Tables
        s_TableFilenames = new List<string>
        {
           "02_RandomStatTable", "06_WeaponTable", "03_LevelTable"
        };
        s_EnforceTableIds = new List<int>
        {
            3011, 3012, 3013
        };
        s_Weapons = new List<Dictionary<int, BaseWeapon>>(size);
        for (int i = 0; i < s_Weapons.Count; ++i)
        {
            s_Weapons[i] = new Dictionary<int, BaseWeapon>();
        }
        WeaponManager.LoadTechData();
    }

    private static void LoadTechData()
    {
        DataTable<RandomStatData>.Init(s_TableFilenames[(int)TableType.RandomStatTable]);
        DataTable<WeaponData>.Init(s_TableFilenames[(int)TableType.WeaponTable]);
        DataTable<LevelData>.Init(s_TableFilenames[(int)TableType.LevelTable]);
        for (int i = 0; i < s_Weapons.Count; ++i)
        {
            int enforceId = s_EnforceTableIds[i];
            RandomStatData ranStatData = DataTable<RandomStatData>.At(enforceId);
            WeaponManager.AttachWeapon(ranStatData, s_Weapons[i]);
        }
    }

    private static BaseWeapon DeepCopy(ref BaseWeapon src)
    {
        BaseWeapon target = null;
        if (src != null)
        {
            target = new BaseWeapon();

            target.id = src.id;
            target.itemName = src.itemName;
            target.attackPower = src.attackPower;
            target.attackSpeed = src.attackSpeed;
            target.basicAttackRange = new List<int>(src.basicAttackRange);
            target.effectKeyIds = new List<int>(src.effectKeyIds);
            target.skillKeyIds = new List<int>(src.skillKeyIds);
            target.monsterMaximumTarget = src.monsterMaximumTarget;
        }
        return target;
    }

    private static void AttachWeapon(RandomStatData ranStatData, Dictionary<int, BaseWeapon> dst)
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
            dst.Add(baseWeapon.id, baseWeapon);
        }
    }

} // static class WeaponManager