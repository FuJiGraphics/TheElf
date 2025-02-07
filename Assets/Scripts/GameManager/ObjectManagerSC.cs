
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

    private void Awake()
    {
        this.Init();
    }

    public GameObject Get()
    {
        this.Init();
        return m_Pool.Get();
    }

    public void Release(GameObject target)
        => m_Pool.Release(target);

    private void Init()
    {
        if (m_Pool != null)
            return;

        m_Pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),   // 새 객체를 생성하는 방법
            actionOnGet: ActionOnGet,                // 풀에서 가져올 때 실행
            actionOnRelease: ActionOnRelease,        // 풀에 반환될 때 실행
            actionOnDestroy: ActionOnDestroy,        // 풀에서 제거될 때 실행
            collectionCheck: false,                  // 중복 반환 검사
            defaultCapacity: 1,                      // 기본 용량
            maxSize: maxCount                        // 최대 용량
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