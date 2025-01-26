using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ObjectPool
{
    public Transform Parent { get; set; } = null;
    public int MaxCount { get; private set; } = 100;
    public int UseCount { get; private set; } = 0;
    public GameObject TargetPrefab { get; private set; } = null;
    public GameObject Instance { get; private set; } = new GameObject("ObjectPool");
    private Dictionary<GameObject, bool> m_GameObjects;
    private bool isChangingState = false;

    public void Init(GameObject prefabs, int maxCount, Transform parent = null)
    {
        if (maxCount < 1)
            return;

        Parent = parent != null ? parent : null;
        TargetPrefab = prefabs;
        MaxCount = maxCount;
        m_GameObjects = new Dictionary<GameObject, bool>();
        for (int i = 0; i < MaxCount; ++i)
        {
            GameObject go = GameObject.Instantiate(TargetPrefab);
            if (Parent)
                go.transform.SetParent(Parent);
            else
                go.transform.SetParent(Instance.transform);
            m_GameObjects.Add(go, false);
            go.SetActive(false);
        }
    }

    public void Release()
    {
        TargetPrefab = null;
        foreach (var obj in m_GameObjects.Keys)
        {
            if (obj != null)
                GameObject.Destroy(obj);
        }
        m_GameObjects.Clear();
        UseCount = 0;
        MaxCount = 0;
        GameObject.Destroy(Instance);
    }

    public void SetParent(Transform transform)
        => Parent = transform;

    public GameObject Gen(Vector3 position)
        => this.Gen(position, Quaternion.identity);

    public GameObject Gen(Vector3 position, Quaternion rotation)
        => this.Gen(position, rotation, Vector3.one);

    public GameObject Gen(Transform transform)
        => this.Gen(transform.position, transform.rotation, transform.localScale);

    public GameObject Gen(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        if (isChangingState)
            return null;

        GameObject target = null;
        if (m_GameObjects != null && UseCount < MaxCount)
        {
            foreach (var obj in m_GameObjects)
            {
                if (obj.Key != null && !obj.Value)
                {
                    m_GameObjects[obj.Key] = true;
                    target = obj.Key;

                    isChangingState = true;
                    if (!target.activeSelf)
                    {
                        target.SetActive(true);
                    }
                    isChangingState = false;

                    target.transform.position = position;
                    target.transform.rotation = rotation;
                    target.transform.localScale= localScale;
                    UseCount++;
                    break;
                }
            }
        }
        return target;
    }

    public void Return(GameObject obj)
    {
        if (m_GameObjects == null || obj == null || MaxCount < 1 || isChangingState)
            return;

        if (m_GameObjects.ContainsKey(obj))
        {
            if (m_GameObjects[obj])
            {
                m_GameObjects[obj] = false;
                isChangingState = true;
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                }
                isChangingState = false;
                obj.transform.position = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                UseCount--;
            }
        }
    }

    public void ReturnAll()
    {
        if (m_GameObjects == null)
            return;

        var keys = new List<GameObject>(m_GameObjects.Keys);
        for (int i = 0; i < keys.Count; ++i)
        {
            this.Return(keys[i]);
        }
    }

} // class ObjectPool
