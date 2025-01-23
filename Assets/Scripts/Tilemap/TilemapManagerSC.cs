using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapManagerSC : MonoBehaviour 
{
    public Grid targetGrid;
    public GameObject targetPrefab;

    private static ObjectPool s_ObjPool;
    private static Dictionary<string, bool> s_ObjMap;

    private bool IsInitialized { get; set; } = false;

    private void Start()
    {
        this.Init();
    }

    public GameObject Get(Vector3 position)
    {
        this.Init();

        string targetKey = position.ToString();
        GameObject resultObject = null;
        if (s_ObjMap.ContainsKey(targetKey))
        {
            if (s_ObjMap[targetKey] == false)
            {
                resultObject = s_ObjPool.Gen(position);
                s_ObjMap[targetKey] = true;
            }
        }
        else
        {
            resultObject = s_ObjPool.Gen(position);
            s_ObjMap.Add(targetKey, true);
        }
        return resultObject;
    }

    public void Return(GameObject obj)
    {
        this.Init();

        string targetKey = obj.transform.position.ToString();
        if (s_ObjMap.ContainsKey(targetKey))
        {
            if (s_ObjMap[targetKey] == true)
            {
                s_ObjMap[targetKey] = false;
                s_ObjPool.Return(obj);
            }
        }
    }

    public void SetParent(Transform transform)
        => s_ObjPool.SetParent(transform);

    private void Init()
    {
        if (IsInitialized)
            return;

        IsInitialized = true;
        s_ObjPool = new ObjectPool();
        s_ObjPool.Init(targetPrefab, 50, targetGrid.transform);
        s_ObjMap = new Dictionary<string, bool>();
    }

} // class TilemapManagerSC
