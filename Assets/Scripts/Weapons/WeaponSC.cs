using UnityEngine;

public class WeaponSC : MonoBehaviour
{
    public Sprite icon;
    public string desc;

    public int level;
    public WeaponType type;
    public int id = 0;
    public string weaponName = "Empty";

    public bool fixedRotation = false;
    public GameObject objectPool;
    public GameObject bulletPrefab;
    public BaseWeapon info;

    private float m_ElapsedTime = 0f;
    private ObjectManagerSC m_ObjectPool;

    private void Start()
    {
        this.Init();
        info = WeaponManager.GetInfo(type, level);
    }

    private void OnEnable()
    {
        this.Init();
        info = WeaponManager.GetInfo(type, level);
    }

    private void OnValidate()
    {
        info = WeaponManager.GetInfo(type, level);

        if (info != null)
        {
            this.Init();
        }
    }

    public void Fire(Vector2 direction, Vector2 position)
     => this.Fire(direction, position, Quaternion.identity);

    public void Fire(Vector2 direction, Vector2 position, Quaternion rotation)
    {
        this.Init();
        info = WeaponManager.GetInfo(type, level);
        LogManager.IsVaild(bulletPrefab);
        if (info == null)
        {
            Debug.LogError($"Did not found data Type:{type}, Level:{level}");
        }
        m_ElapsedTime += Time.deltaTime;
        if (m_ElapsedTime >= info.attackSpeed)
        {
            m_ElapsedTime = 0f;
            GameObject bullet;
            if (fixedRotation == true)
            {
                rotation = Quaternion.identity;
            }

            if (m_ObjectPool != null)
            {
                bullet = m_ObjectPool.Get();
            }
            else
            {
                bullet = GameObject.Instantiate(bulletPrefab, position, Quaternion.identity);
            }
            BulletSC sc = bullet.GetComponent<BulletSC>();
            sc.ownerPool = m_ObjectPool != null ? m_ObjectPool : null;
            sc.SetWeaponData(info);
            sc.Fire(position, direction, rotation);
        }
    }

    private void Init()
    {
        if (info != null)
        {
            this.id = info.id;
            this.weaponName = info.itemName;
            m_ObjectPool = GetComponentInChildren<ObjectManagerSC>();
            if (m_ObjectPool != null)
            {
                m_ObjectPool.prefab = bulletPrefab != null ? bulletPrefab : null;
                objectPool = m_ObjectPool.gameObject;
            }
        }
    }

} // class WeaponSC
