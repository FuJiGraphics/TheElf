using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public string label = "Spawner";    // 라벨
    public int id = 0;                  // 스포너 ID
    public int monsterId = 0;           // 몬스터 ID
    public float respawnCycle = 0f;     // 리스폰 간격
    public int maxCount = 0;            // 리스폰 생성 최대 개수
    public float offset = 0f;           // 리스폰 오프셋
    public float duration = 0f;         // 리스폰 지속 시간

    ObjectSpawner spawner;

    void Start()
    {
        spawner = GetComponent<ObjectSpawner>();
        if (spawner == null)
        {
            Debug.LogError("Spawner is null");
        }
    }

    void Update()
    {
        
    }
}
