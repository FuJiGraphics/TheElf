using System.Collections.Generic;
using UnityEngine;

public static class DataTable<T>
    where T : IGameData
{
    private static Dictionary<int, T> s_Table;
    private static bool s_IsInitialized = false;

    public static string FileName { get; set; } = "Empty";

    public static void Init(string fileName)
    {
        if (s_IsInitialized)
            return;
        s_IsInitialized = true;
        s_Table = new Dictionary<int, T>();

        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다.");
        }

        var records = CsvManager.LoadFromText<T>(csvFile.text);
        foreach (T data in records)
        {
            if (s_Table.ContainsKey(data.Id))
            {
                Debug.Log($"중복 선언된 키입니다. {data.Id}");
                continue;
            }
            s_Table.Add(data.Id, data);
        }
    }

    public static T At(int id)
    {
        if (id < 0)
        {
            Debug.LogError("인덱스를 찾을 수 없습니다.");
            return default(T);
        }
        if (!s_IsInitialized)
        {
            Debug.LogError("초기화 되지 않은 테이블입니다.");
            return default(T);
        }
        if (!s_Table.ContainsKey(id))
        {
            Debug.LogError($"Id를 찾을 수 없습니다. {id}");
            return default(T);
        }
        return s_Table[id];
    }

} // class EnemyTable