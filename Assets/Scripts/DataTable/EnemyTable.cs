using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class EnemyTable
{
    private static Dictionary<int, MonsterData> s_Table;
    private static bool m_IsInitialized = false;

    public static MonsterData Get(int id)
    {
        EnemyTable.Init();
        return s_Table[id];
    }

    public static MonsterData At(int id)
    {
        EnemyTable.Init();
        if (!s_Table.ContainsKey(id))
        {
            Debug.LogError($"Id를 찾을 수 없습니다. {id}");
            return null;
        }
        return s_Table[id];
    }

    private static void Init()
    {
        if (m_IsInitialized)
            return;
        m_IsInitialized = true;
        s_Table = new Dictionary<int, MonsterData>();

        TextAsset csvFile = Resources.Load<TextAsset>("04_MonsterTable");
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다.");
        }

        var records = CsvManager.LoadFromText<MonsterData>(csvFile.text);
        foreach (MonsterData monster in records)
        {
            if (s_Table.ContainsKey(monster.Id))
            {
                Debug.Log($"중복 선언된 키입니다. {monster.Id}");
                continue;
            }
            s_Table.Add(monster.Id, monster);
        }
    }

} // class EnemyTable