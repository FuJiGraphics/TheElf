using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapSC : MonoBehaviour
{
    public TilemapManagerSC tilemapManager;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tilemapManager.Return(gameObject);
        }
    }
} // class TilemapSC
