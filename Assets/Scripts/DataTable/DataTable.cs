using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

public static class DataTable<T>
    where T : IGameData
{
    private static Dictionary<string, T> s_Table;

    public static string FileName { get; set; } = "Empty";

    public static void Init(string fileName)
    {
        if (s_Table == null)
        {
            s_Table = new Dictionary<string, T>();
        }

        s_Table = new Dictionary<string, T>();

        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError("did not found csv file!");
        }

        var records = CsvManager.LoadFromText<T>(csvFile.text);
        foreach (T data in records)
        {
            if (s_Table.ContainsKey(data.Id))
            {
                s_Table.Remove(data.Id);
            }
            s_Table.Add(data.Id, data);
        }
    }

    public static bool InitFromPersistentData(string fileName)
    {
        if (s_Table == null)
        {
            s_Table = new Dictionary<string, T>();
        }

        if (fileName.Contains(".csv") == false)
        {
            fileName += ".csv";
        }
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            var records = CsvManager.Load<T>(filePath);
            foreach (T data in records)
            {
                if (s_Table.ContainsKey(data.Id))
                {
                    s_Table.Remove(data.Id);
                }
                s_Table.Add(data.Id, data);
            }
        }
        else
        {
            Debug.LogWarning($"Unexpected Error. {filePath}");
            return false;
        }
        return true;
    }

    public static void SaveFromPersistentData(string fileName, T data)
    {
        CsvManager.SaveInPersistentDataPath(data, fileName);
        InitFromPersistentData(fileName);
    }

    public static void Release()
    {
        s_Table.Clear();
        s_Table = null;
    }

    public static T At(string id)
    {
        if (s_Table == null)
        {
            Debug.LogError("초기화되지 않은 테이블입니다.");
            return default(T);
        }
        if (!s_Table.ContainsKey(id))
        {
            Debug.LogError($"아이디를 찾을 수 없습니다 {id}");
            return default(T);
        }
        return s_Table[id];
    }

    public static bool Exists(string id)
        => s_Table != null && s_Table.ContainsKey(id);

} // class EnemyTable