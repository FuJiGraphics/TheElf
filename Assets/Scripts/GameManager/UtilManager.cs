
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilManager
{
    public static bool CheckProbability(float chance)
    {
        float randomValue = UnityEngine.Random.Range(0.0f, 100.0f);
        return randomValue < chance;  // 확률에 맞게 true/false 반환
    }

    public static Dictionary<T1, T2> Sort<T1, T2>(Dictionary<T1, T2> src, bool isAscending = true)
    {
        Dictionary<T1, T2> result = new Dictionary<T1, T2>();

        var sortList = src.Keys.ToList<T1>();
        if (isAscending)
        {
            sortList.Sort();
        }
        else
        {
            sortList.Reverse();
        }

        foreach (var key in sortList)
        {
            result.Add(key, src[key]);
        }
        return result;
    }

    public static void Copy<T>(out List<T> dst, ref List<T> src)
    {
        dst = new List<T>(src.Count);
        dst.AddRange(src);
    }

    public static GameObject FindWithName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject result = null;
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name)
            {
                result = obj;
                break;
            }
        }
        return result;
    }

} // class static class UtilManager