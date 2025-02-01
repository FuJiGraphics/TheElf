using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTable : Singleton<EnemyTable>
{
    private Dictionary<int, MonsterData> m_Table;

    private void Awake()
    {
        m_Table = new Dictionary<int, MonsterData>();
        string path = "/DataTables/04_MonsterTable.csv";
        var records = CsvManager.Load<MonsterData>(Application.dataPath + path);
        foreach (MonsterData monster in records)
        {
            if (m_Table.ContainsKey(monster.Id))
            {
                Debug.Log($"�ߺ� ����� Ű�Դϴ�. {monster.Id}");
                continue;
            }
            m_Table.Add(monster.Id, monster);
        }
    }

    public MonsterData Get(int id)
        => m_Table[id];

    public MonsterData At(int id)
    {
        if (!m_Table.ContainsKey(id))
        {
            Debug.LogError($"Id�� ã�� �� �����ϴ�. {id}");
            return null;
        }
        return m_Table[id];
    }

} // class EnemyTable