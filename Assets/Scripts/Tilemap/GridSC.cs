using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSC : MonoBehaviour
{
    public TilemapManagerSC tilemapManager;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 upVec = (Vector3.up * 33f);
        tilemapManager.Get(playerPos);
        tilemapManager.Get(playerPos + upVec);
    }
} // class GridSC
