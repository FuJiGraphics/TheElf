
using UnityEngine.Pool;
using UnityEngine;
using UnityEngine.Events;

public class ObjectManagerSC: MonoBehaviour
{
    public GameObject prefab;
    public int maxCount;
    public UnityEvent<GameObject> onGet;
    public UnityEvent<GameObject> onRelease;
    public UnityEvent<GameObject> onDestroy;

    private ObjectPool<GameObject> m_Pool;
    private bool m_IsInitialized = false;

    private void Awake()
    {
        this.Init();
    }

    public GameObject Get()
    {
        this.Init();
        return m_Pool.Get();
    }

    public void Release(GameObject)

    private void Init()
    {
        if (m_IsInitialized)
            return;
        m_IsInitialized = true;

        m_Pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),   // �� ��ü�� �����ϴ� ���
            actionOnGet: ActionOnGet,                // Ǯ���� ������ �� ����
            actionOnRelease: ActionOnRelease,        // Ǯ�� ��ȯ�� �� ����
            actionOnDestroy: ActionOnDestroy,        // Ǯ���� ���ŵ� �� ����
            collectionCheck: false,                  // �ߺ� ��ȯ �˻�
            defaultCapacity: maxCount,               // �⺻ �뷮
            maxSize: maxCount                        // �ִ� �뷮
        );
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.SetActive(true);
        onGet.Invoke(obj);
    }

    private void ActionOnRelease(GameObject obj)
    {
        obj.SetActive(false);
        onRelease.Invoke(obj);
    }

    private void ActionOnDestroy(GameObject obj)
    {
        Destroy(obj);
        onDestroy.Invoke(obj);
    }

} // static class BulletManager