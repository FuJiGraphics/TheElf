using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSC : MonoBehaviour
{
    public TilemapManagerSC tilemapManager;

    private void Start()
    {
        tilemapManager.Get(Vector3.zero);
        tilemapManager.Get(Vector3.up * 33f);
    }
} // class GridSC
