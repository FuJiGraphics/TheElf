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
    public float offset = 0f;           // ������ ������
    public float duration = 0f;         // ������ ���� �ð�

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
