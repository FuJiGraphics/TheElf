using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

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

    [Tooltip("X: Min, Y: Max")]
    public Vector2 randomCubeSize = new Vector2 { x = 5f, y = 5f };
    public float randomSphereRadius = 5f;
    public GameObject[] spawnTargets;
    public UnityEvent<GameObject> spawnEvents;

    private ObjectPool[] objectPools;

    public GameObject Spawn()
        => this.Spawn(transform.position);

    public GameObject Spawn(Vector3 position)
    {
        GameObject newObject = null;
        for (int i = 0; i < spawnTargets.Length; ++i)
        {
            newObject = this.Spawn(position, Quaternion.identity);
        }
        return newObject;
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject newObject = null;
        for (int i = 0; i < spawnTargets.Length; ++i)
        {
            newObject = this.Spawn(i, position, rotation);
        }
        return newObject;
    }

    public GameObject Spawn(int index, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = null;
        if (spawnTargets.Length > 0)
        {
            newObject = objectPools[index].Gen(position, rotation);
            spawnEvents?.Invoke(newObject);
        }
        return newObject;
    }

    public void RandomSpawnFromSphere()
    {
        if (spawnTargets.Length <= 0)
            return;

        this.RandomSpawnFromSphere(FixedValue.None);
    }

    public void RandomSpawnFromSphere(FixedValue flag)
    {
        if (spawnTargets.Length <= 0)
            return;

        int index = 0;
        index = UnityEngine.Random.Range(0, spawnTargets.Length);
        this.RandomSpawnFromSphere(index, flag);
    }

    public void RandomSpawnFromSphere(int index, FixedValue flag = FixedValue.None)
    {
        if (spawnTargets.Length <= 0)
            return;

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

        GameObject newObject = this.Spawn(index, ranPos, ranRot);
        if (newObject != null)
        {
            newObject.GetComponent<Renderer>().material.color = ranColor;
        }
    }

    public void RandomSpawnFromCube()
    {
        if (spawnTargets.Length <= 0)
            return;

        this.RandomSpawnFromCube(FixedValue.None);
    }

    public void RandomSpawnFromCube(FixedValue flag)
    {
        if (spawnTargets.Length <= 0)
            return;

        int index = 0;
        index = UnityEngine.Random.Range(0, spawnTargets.Length);
        this.RandomSpawnFromCube(index, flag);
    }

    public void RandomSpawnFromCube(int index, FixedValue flag = FixedValue.None)
    {
        if (spawnTargets.Length <= 0)
            return;

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

        GameObject newObject = this.Spawn(index, ranPos, ranRot);
        if (newObject != null)
        {
            newObject.GetComponent<Renderer>().material.color = ranColor;
        }
    }

    public void ReleaseSpawnedObjects()
    {
        for (int i = 0; i < objectPools.Length; ++i)
        {
            objectPools[i].ReturnAll();
        }
    }

    private void Start()
    {
        objectPools = new ObjectPool[spawnTargets.Length];
        for (int i = 0; i < objectPools.Length; ++i)
        {
            objectPools[i] = new ObjectPool();
            objectPools[i].Init(spawnTargets[i], 10);
            objectPools[i].Instance.transform.SetParent(transform);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < objectPools.Length; ++i)
        {
            objectPools[i].Release();
        }
    }

} // class ObjectSpawner
