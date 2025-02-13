using Helios.GUI;
using UnityEngine;

public class WeaponSC : MonoBehaviour
{
    public Sprite icon;
    public string desc;

    public int level;
    public WeaponType type;
    public string id = "0";
    public string weaponName = "Empty";
    public bool isAutoTarget = false;

    public bool fixedRotation = false;
    public GameObject objectPool;
    public GameObject bulletPrefab;
    public Effects effects;
    public BaseWeapon info;

    private float m_ElapsedTime = 0f;
    private ObjectManagerSC m_ObjectPool;
    private GameObject m_Owner;

    private void Start()
    {
        this.Init();
        info = WeaponManager.Instance.GetInfo(type, level);
    }

    private void OnEnable()
    {
        if (GameManagerSC.Instance.IsSceneLoaded == false)
            return;

        this.Init();
        info = WeaponManager.Instance.GetInfo(type, level);
    }

    public void Fire(Vector2 direction, Vector2 position)
     => this.Fire(direction, position, Quaternion.identity, null);

    public void Fire(Vector2 direction, Vector2 position, Quaternion rotation, GameObject playerOwner)
    {
        this.Init();
        info = WeaponManager.Instance.GetInfo(type, level);
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
            if (isAutoTarget)
            {
                Vector2 newDir = this.FindAutoTarget(ref direction, ref position);
                if (newDir != Vector2.zero)
                {
                    direction = newDir;
                    rotation = Quaternion.LookRotation(Vector3.forward, direction);
                }
            }
            m_Owner = playerOwner;
            sc.Fire(position, direction, rotation, playerOwner);
            effects?.Play(position, rotation);
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
            m_Owner = GameObject.FindWithTag("Player");
            effects = GetComponentInChildren<Effects>();
        }
    }

    private Vector2 FindAutoTarget(ref Vector2 direction, ref Vector2 pos)
    {
        Vector2 dir = Vector2.zero;
        GameObject enemyGo = GameManagerSC.Instance.RandomEnemyTarget;
        if (enemyGo != null)
        {
            Vector3 currPos = pos;
            dir = (enemyGo.transform.position - currPos).normalized;
        }
        return dir;
    }


} // class WeaponSC
