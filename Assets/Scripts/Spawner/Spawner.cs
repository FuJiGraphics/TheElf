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
    public float offset = 0f;           // 리스폰 시간 오프셋
    public float duration = 0f;         // 리스폰 지속 시간
    public GameObject trackingTarget;   // 추적 대상

    ObjectSpawner m_Spawner;
    WaitForSeconds m_WaitRespawnCycle;
    WaitForSeconds m_WaitRespawnOffset;
    float m_ElapsedTime;

    void Start()
    {
        m_Spawner = GetComponent<ObjectSpawner>();
        if (m_Spawner == null)
        {
            Debug.LogError("Spawner is null");
        }
        this.StartSpawner();
    }
    
    void Update()
    {
        if (!GameManagerSC.Instance.IsPlaying)
            return;

        m_ElapsedTime += Time.deltaTime;    
    }

    public void StartSpawner()
    {
        m_WaitRespawnCycle = new WaitForSeconds(respawnCycle);
        m_WaitRespawnOffset = new WaitForSeconds(offset);
        StartCoroutine(RespawnCoroutine());
    }

    public void StopSpawner()
    {
        StopCoroutine(RespawnCoroutine());
        m_ElapsedTime = 0f;
    }

    IEnumerator RespawnCoroutine()
    {
        yield return m_WaitRespawnOffset;
        while (m_ElapsedTime < duration)
        {
            GameObject go = m_Spawner.Spawn();
            EnemySC sc = go.GetComponent<EnemySC>();
            if (sc != null)
            {
                sc.target = trackingTarget;
            }
            yield return m_WaitRespawnCycle;
        }
    }

} // class Spawner
