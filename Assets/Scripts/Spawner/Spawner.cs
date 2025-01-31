using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public string label = "Spawner";    // ��
    public int id = 0;                  // ������ ID
    public int monsterId = 0;           // ���� ID
    public float respawnCycle = 0f;     // ������ ����
    public int maxCount = 0;            // ������ ���� �ִ� ����
    public float offset = 0f;           // ������ �ð� ������
    public float duration = 0f;         // ������ ���� �ð�
    public GameObject trackingTarget;   // ���� ���

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
