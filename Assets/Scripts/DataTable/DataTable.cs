using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTable
{
    private static Dictionary<int, GameObject> s_Table;
    private static bool s_Initialized = false;

    public static int Count { get => s_Table.Count; }

    public static void Init()
    {
        if (s_Initialized)
            return; 

        s_Initialized = true;
        s_Table = new Dictionary<int, GameObject>();
    }

    public static bool LoadFromCSV(string path)
    {
        bool result = false;



        return result;
    }

    public static GameObject At(int index)
    {
        DataTable.Init();

        GameObject findGo = null;
        if (s_Table.ContainsKey(index))
        {
            findGo = s_Table[index];
        }
        return findGo;
    }

    public static GameObject Get(int index)
    {
        DataTable.Init();
        return s_Table[index];
    }

} // class DataTable;
