
using System;
using System.Collections.Generic;
using System.Linq;

public static class UtilManager
{
    public static bool CheckProbability(float chance)
    {
        float randomValue = UnityEngine.Random.Range(0.0f, 100.0f);
        return randomValue < chance;  // Ȯ���� �°� true/false ��ȯ
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

} // class static class UtilManager