
using System;
using System.Collections.Generic;

public static class LevelManager
{
    private static readonly string s_RandomStatFilename = "02_RandomStatTable";
    private static readonly string s_WeaponDataFilename = "06_WeaponTable";
    private static readonly string s_LevelDataFilename = "03_LevelTable";
    private static readonly int s_LongbowId = 3011;
    private static readonly int s_CrossbowId = 3012;
    private static readonly int s_SwordId = 3013;
    private static Dictionary<int, Longbow> s_LongbowTech;
    private static Dictionary<int, Crossbow> s_CrossbowTech;
    private static Dictionary<int, Sword> s_SwordTech;
    private static bool s_IsInitialized = false;

    public static Longbow LevelUp(Longbow longbow)
    {
        LevelManager.Init();
        int nextLevel = longbow.WeaponLevel + 1;
        return s_LongbowTech[nextLevel];
    }
    public static Crossbow LevelUp(Crossbow crossbow)
    {
        LevelManager.Init();
        int nextLevel = crossbow.WeaponLevel + 1;
        return s_CrossbowTech[nextLevel];
    }
    public static Sword LevelUp(Sword sword)
    {
        LevelManager.Init();
        int nextLevel = sword.WeaponLevel + 1;
        return s_SwordTech[nextLevel];
    }

    private static void Init()
    {
        if (s_IsInitialized)
            return;
        s_IsInitialized = true;
        s_LongbowTech = new Dictionary<int, Longbow>();
        s_CrossbowTech = new Dictionary<int, Crossbow>();
        s_SwordTech = new Dictionary<int, Sword>();
        LevelManager.LoadTechData();
    }

    private static void LoadTechData()
    {
        DataTable<RandomStatData>.Init(s_RandomStatFilename);
        DataTable<WeaponData>.Init(s_WeaponDataFilename);
        var longbowData = DataTable<RandomStatData>.Get(s_LongbowId);
        var crossbowData = DataTable<RandomStatData>.Get(s_CrossbowId);
        var swordData = DataTable<RandomStatData>.Get(s_SwordId);
        LevelManager.AttachWeapon<Longbow>(longbowData);
        LevelManager.AttachWeapon<Crossbow>(crossbowData);
        LevelManager.AttachWeapon<Sword>(swordData);
    }

    private static void AttachWeapon<T>(RandomStatData ranStatData)
    {
        List<int> itemList = CsvManager.ToList<int>(ranStatData.ItemPayments);
        List<int> levelIds = CsvManager.ToList<int>(ranStatData.LevelKeyIds);
        DataTable<LevelData>.Init(s_LevelDataFilename);
        if (itemList.Count != levelIds.Count)
        {
            // 아이템 개수와 레벨의 개수가 대응되지 않음
            throw new Exception();
        }
        for (int i = 0; i < itemList.Count; ++i)
        {
            int id = itemList[i];
            LevelData levelData = DataTable<LevelData>.Get(levelIds[i]);
            var weaponData = DataTable<WeaponData>.Get(id);
            BaseWeapon baseWeapon = LevelManager.GetTypeBase<T>();
            baseWeapon.id = weaponData.Id;
            baseWeapon.itemName = weaponData.Name;
            baseWeapon.attackPower = weaponData.BasicAttack;
            baseWeapon.attackSpeed = weaponData.BasicAttackSpeed;
            baseWeapon.basicAttackRange = CsvManager.ToList<int>(weaponData.BasicAttackRange);
            baseWeapon.monsterMaximumTarget = weaponData.MonsterMaximumTarget;
            baseWeapon.effectKeyIds = CsvManager.ToList<int>(weaponData.EffectKeyIds);
            baseWeapon.skillKeyIds = CsvManager.ToList<int>(weaponData.SkillKeyIds);
            baseWeapon.WeaponLevel = levelData.Level;
            LevelManager.SetTypeBase<T>(baseWeapon);
        }
    }

    private static BaseWeapon GetTypeBase<T>()
    {
        if (typeof(T) == typeof(Longbow))
            return new Longbow();
        else if (typeof(T) == typeof(Crossbow))
            return new Crossbow();
        else if (typeof(T) == typeof(Sword))
            return new Sword();
        return null;
    }

    private static void SetTypeBase<T>(BaseWeapon target)
    {
        Longbow longbow;
        Crossbow crossbow;
        Sword sword;
        if (typeof(T) == typeof(Longbow))
        {
            longbow = (Longbow)Convert.ChangeType(target, typeof(Longbow));
            s_LongbowTech.Add(target.id, longbow);
        }
        else if (typeof(T) == typeof(Crossbow))
        {
            crossbow = (Crossbow)Convert.ChangeType(target, typeof(Crossbow));
            s_CrossbowTech.Add(target.id, crossbow);
        }
        else if (typeof(T) == typeof(Sword))
        {
            sword = (Sword)Convert.ChangeType(target, typeof(Sword));
            s_SwordTech.Add(target.id, sword);
        }
    }

} // static class LevelManager