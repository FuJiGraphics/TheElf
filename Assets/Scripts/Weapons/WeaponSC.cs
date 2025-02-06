using UnityEngine;

public class WeaponSC : MonoBehaviour
{
    public Sprite icon;
    public string desc;

    public GameObject objectPool;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    public BaseWeapon data;
    public WeaponType type;

    private float m_ElapsedTime = 0f;
    private ObjectManagerSC m_ObjectPool;

    private void Awake()
    {
        data = WeaponManager.Get(type, 0);
        if (objectPool != null)
        {
            m_ObjectPool = objectPool.GetComponent<ObjectManagerSC>();
        }
        if (bulletPrefab == null)
        {
            Debug.LogError("Prefab is null!!");
        }
    }
    public void Fire(Vector2 direction, Vector2 position)
     => this.Fire(direction, position, Quaternion.identity);

    public void Fire(Vector2 direction, Vector2 position, Quaternion rotation)
    {
        m_ElapsedTime += Time.deltaTime;
        if (m_ElapsedTime >= data.attackSpeed)
        {
            m_ElapsedTime = 0f;
            if (m_ObjectPool != null)
            {
                objectPool.Get();
            }
            else
            {
                Vector2.Angle(position, position + direction);
                GameObject bullet = GameObject.Instantiate(bulletPrefab, position, rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }
        }
    }

} // class WeaponSC
