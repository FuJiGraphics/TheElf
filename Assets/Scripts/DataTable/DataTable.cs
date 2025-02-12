using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

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
            Debug.LogError("CSV ������ ã�� �� �����ϴ�.");
        }

        var records = CsvManager.LoadFromText<T>(csvFile.text);
        foreach (T data in records)
        {
            if (s_Table.ContainsKey(data.Id))
            {
                Debug.Log($"�ߺ� ����� Ű�Դϴ�. {data.Id}");
                continue;
            }
            s_Table.Add(data.Id, data);
        }
    }

    public static bool InitFromPersistentData(string fileName)
    {
        if (s_IsInitialized)
            return false;
        s_IsInitialized = true;

        s_Table = new Dictionary<int, T>();

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
                    Debug.Log($"�ߺ� ����� Ű�Դϴ�. {data.Id}");
                    continue;
                }
                s_Table.Add(data.Id, data);
            }
        }
        else
        {
            Debug.LogError($"CSV ������ ã�� �� �����ϴ�. {filePath}");
            return false;
        }
        return true;
    }

    public static void Release()
    {
        s_IsInitialized = false;
        s_Table.Clear();
        s_Table = null;
    }

    public static T At(int id)
    {
        if (id < 0)
        {
            Debug.LogError("�ε����� ã�� �� �����ϴ�.");
            return default(T);
        }
        if (!s_IsInitialized)
        {
            Debug.LogError("�ʱ�ȭ ���� ���� ���̺��Դϴ�.");
            return default(T);
        }
        if (!s_Table.ContainsKey(id))
        {
            Debug.LogError($"Id�� ã�� �� �����ϴ�. {id}");
            return default(T);
        }
        return s_Table[id];
    }

    public static bool Exists(int id)
        => s_Table.ContainsKey(id);

} // class EnemyTable