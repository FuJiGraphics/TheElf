using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    private readonly int s_LongbowId = 3011;
    private readonly int s_CrossbowId = 3012;
    private readonly int s_SwordId = 3013;

    private List<int> m_WeaponIds;
    private List<Dictionary<int, BaseWeapon>> m_Weapons;

    public BaseWeapon GetInfo(WeaponType type, int level)
    {
        BaseWeapon result = null;
        int weaponType = (int)type;
        if (weaponType < m_Weapons.Count && m_Weapons[weaponType].ContainsKey(level))
        {
            var weapon = m_Weapons[weaponType][level];
            result = new BaseWeapon();
            result.DeepCopy(ref weapon);
        }
        return result;
    }

    public GameObject LevelUp(GameObject weapon)
    {
        GameObject result = null;
        WeaponSC sc = weapon.GetComponent<WeaponSC>();
        if (sc != null)
        {
            int nextLevel = sc.info.WeaponLevel + 1;
            int targetWeapon = (int)sc.info.WeaponType;
            if (m_Weapons[targetWeapon].ContainsKey(nextLevel))
            {
                BaseWeapon src = m_Weapons[targetWeapon][nextLevel];
                result = UtilManager.FindWithName(src.itemName);
            }
        }
        return result;
    }

    public void Init()
    {
        if (m_Weapons != null)
        {
            bool hasNull = false;
            for (int i = 0; i < m_Weapons.Count; ++i)
            {
                hasNull = m_Weapons[i] == null;
                if (hasNull)
                    break;
            }
            if (!hasNull)
            {
                return;
            }
        }

        int size = (int)WeaponType.Max;
        m_WeaponIds = new List<int>(size)
        {
            s_LongbowId, s_CrossbowId, s_SwordId
        };
        m_Weapons = new List<Dictionary<int, BaseWeapon>>(size);
        for (int i = 0; i < size; ++i)
        {
            m_Weapons.Add(new Dictionary<int, BaseWeapon>());
            this.LoadTechData((WeaponType)i);
        }
    }

    public void Release()
    {
        m_Weapons.Clear();
        m_Weapons = null;
    }

    private void LoadTechData(WeaponType type)
    {
        var data = DataTable<RandomStatData>.At(m_WeaponIds[(int)type]);
        this.AttachWeapon(type, data);
    }

    private void AttachWeapon(WeaponType type, RandomStatData ranStatData)
    {
        List<int> itemList = CsvManager.ToList<int>(ranStatData.ItemPayments);
        List<int> levelIds = CsvManager.ToList<int>(ranStatData.LevelKeyIds);
        if (itemList.Count != levelIds.Count)
        {
            // ������ ������ ������ ������ �������� ����
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
            baseWeapon.Duration = weaponData.Duration;
            baseWeapon.Count = weaponData.Count;
            baseWeapon.Bounce = weaponData.Bounce;
            baseWeapon.RotationSpeed = weaponData.RotationSpeed;
            baseWeapon.DamagePerSecond = weaponData.DamagePerSecond;
            baseWeapon.WeaponLevel = levelData.Level;
            baseWeapon.WeaponType = type;
            m_Weapons[(int)type].Add(levelData.Level, baseWeapon);
        }
    }

} // static class LevelManager