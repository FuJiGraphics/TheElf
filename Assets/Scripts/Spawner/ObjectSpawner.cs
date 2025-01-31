using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class ObjectSpawner : MonoBehaviour
{
    [Flags]
    public enum FixedValue
    {
        None        = 0,
        Position    = 1 << 0,
        Rotation    = 1 << 1,
        Color       = 1 << 3,
    }

    public GameObject spawnTarget;
   
    public int defaultCapacity = 10;
    public int maxSize = 20;
    public Vector2 randomCubeSize = new Vector2 { x = 5f, y = 5f };
    public float randomSphereRadius = 5f;
    public UnityEvent<GameObject> spawnEvents;

    private ObjectPool<GameObject> objectPools;

    public void Clear() 
        => objectPools.Clear();

    public GameObject Spawn()
        => this.Spawn(transform.position);

    public GameObject Spawn(Vector3 position)
        => this.Spawn(position, Quaternion.identity);

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject go = objectPools.Get();
        go.transform.position = position;
        go.transform.rotation = rotation;
        spawnEvents?.Invoke(go);
        return go;
    }

    public void RandomSpawnFromSphere()
        => this.RandomSpawnFromSphere(FixedValue.None);

    public void RandomSpawnFromSphere(FixedValue flag = FixedValue.None)
    {
        Vector3 ranPos = Vector3.zero;
        Quaternion ranRot = Quaternion.identity;
        Color ranColor = Color.white;

        if ((flag & FixedValue.Position) != FixedValue.Position)
        {
            ranPos = UnityEngine.Random.insideUnitSphere * randomSphereRadius;
            ranPos += transform.position;
        }

        if ((flag & FixedValue.Rotation) != FixedValue.Rotation)
        {
            ranRot = Quaternion.Euler(
                UnityEngine.Random.Range(0, 360f),
                UnityEngine.Random.Range(0, 360f),
                UnityEngine.Random.Range(0, 360f));
        }

        if ((flag & FixedValue.Color) != FixedValue.Color)
        {
            ranColor = new Color
            {
                r = UnityEngine.Random.value,
                g = UnityEngine.Random.value,
                b = UnityEngine.Random.value,
                // a = UnityEngine.Random.value,
            };
        }

        GameObject newObject = this.Spawn(ranPos, ranRot);
        if (newObject != null)
        {
            newObject.GetComponent<Renderer>().material.color = ranColor;
        }
    }

    public void RandomSpawnFromCube()
        => this.RandomSpawnFromCube(FixedValue.None);

    public void RandomSpawnFromCube(FixedValue flag = FixedValue.None)
    {
        Vector3 ranPos = Vector3.zero;
        Quaternion ranRot = Quaternion.identity;
        Color ranColor = Color.white;

        if ((flag & FixedValue.Position) != FixedValue.Position)
        {
            ranPos.x = UnityEngine.Random.Range(randomCubeSize.x, randomCubeSize.y);
            ranPos.y = UnityEngine.Random.Range(randomCubeSize.x, randomCubeSize.y);
            ranPos.z = UnityEngine.Random.Range(randomCubeSize.x, randomCubeSize.y);
            ranPos += transform.position;
        }

        if ((flag & FixedValue.Rotation) != FixedValue.Rotation)
        {
            ranRot = Quaternion.Euler(
                UnityEngine.Random.Range(0, 360f),
                UnityEngine.Random.Range(0, 360f),
                UnityEngine.Random.Range(0, 360f));
        }

        if ((flag & FixedValue.Color) != FixedValue.Color)
        {
            ranColor = new Color
            {
                r = UnityEngine.Random.value,
                g = UnityEngine.Random.value,
                b = UnityEngine.Random.value,
                // a = UnityEngine.Random.value,
            };
        }

        GameObject newObject = this.Spawn(ranPos, ranRot);
        if (newObject != null)
        {
            newObject.GetComponent<Renderer>().material.color = ranColor;
        }
    }

    private void Start()
    {
        objectPools = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(spawnTarget),     // 새 객체를 생성하는 방법
            actionOnGet: obj => obj.SetActive(true),        // 풀에서 가져올 때 실행
            actionOnRelease: obj => obj.SetActive(false),   // 풀에 반환될 때 실행
            actionOnDestroy: obj => Destroy(obj),           // 풀에서 제거될 때 실행
            collectionCheck: false,                         // 중복 반환 검사
            defaultCapacity: defaultCapacity,               // 기본 용량
            maxSize: maxSize                                // 최대 용량
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, randomCubeSize);
        Gizmos.DrawWireSphere(transform.position, randomSphereRadius);
    }

} // class ObjectSpawner
