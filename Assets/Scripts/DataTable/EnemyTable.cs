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
            Debug.LogError($"Id�� ã�� �� �����ϴ�. {id}");
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
            Debug.LogError("CSV ������ ã�� �� �����ϴ�.");
        }

        var records = CsvManager.LoadFromText<MonsterData>(csvFile.text);
        foreach (MonsterData monster in records)
        {
            if (s_Table.ContainsKey(monster.Id))
            {
                Debug.Log($"�ߺ� ����� Ű�Դϴ�. {monster.Id}");
                continue;
            }
            s_Table.Add(monster.Id, monster);
        }
    }

} // class EnemyTable