using System.Collections.Generic;
using UnityEngine;

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

    // For Enforce Panel
    public Sprite icon;
    public string desc;

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    private float elapsedTime = 0f;
    private bool m_IsInitialized = false;

    public WeaponType WeaponType { get; set; } = WeaponType.Default;
    public int WeaponLevel { get; set; } = 0;

    public BaseWeapon()
    {
        this.Init();
    }

    public virtual void Shoot(Vector2 dir, Vector2 firePoint)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= attackSpeed)
        {
            GameObject bullet = GameObject.Instantiate(projectilePrefab, firePoint, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;

            elapsedTime = 0f;
        }
    }

    private void Init(string tableName = "06_WeaponTable")
    {
        if (m_IsInitialized)
            return;
        m_IsInitialized = true;

        DataTable<WeaponData>.Init(tableName);
        this.SetWeaponData(DataTable<WeaponData>.Get(id));
    }

    private void SetWeaponData(WeaponData weaponData)
    {
        itemName = weaponData.Name;
        attackPower = weaponData.BasicAttack;
        attackSpeed = weaponData.BasicAttackSpeed;
        basicAttackRange = CsvManager.ToList<int>(weaponData.BasicAttackRange);
        monsterMaximumTarget = weaponData.MonsterMaximumTarget;
        effectKeyIds = CsvManager.ToList<int>(weaponData.EffectKeyIds);
        skillKeyIds = CsvManager.ToList<int>(weaponData.SkillKeyIds);
    }

} // class PlayerWeaponSC
