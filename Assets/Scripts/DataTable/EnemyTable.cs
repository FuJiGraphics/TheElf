using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTable : Singleton<EnemyTable>
{
    private Dictionary<int, MonsterData> m_Table;

    private bool m_IsInitialized = false;

    public MonsterData Get(int id)
    {
        this.Init(); 
        return m_Table[id];
    }

    public MonsterData At(int id)
    {
        this.Init();
        if (!m_Table.ContainsKey(id))
        {
            Debug.LogError($"Id를 찾을 수 없습니다. {id}");
            return null;
        }
        return m_Table[id];
    }

    private void Init()
    {
        if (m_IsInitialized)
            return;
        m_IsInitialized = true;
        m_Table = new Dictionary<int, MonsterData>();
        string path = "/DataTables/04_MonsterTable.csv";
        var records = CsvManager.Load<MonsterData>(Application.dataPath + path);
        foreach (MonsterData monster in records)
        {
            if (m_Table.ContainsKey(monster.Id))
            {
                Debug.Log($"중복 선언된 키입니다. {monster.Id}");
                continue;
            }
            m_Table.Add(monster.Id, monster);
        }
    }

} // class EnemyTable