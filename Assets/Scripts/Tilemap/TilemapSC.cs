using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSC : MonoBehaviour
{
    public EmptyAreaSC[] areas;

    private void Awake()
    {
        
    }

    private void Update()
    {
    }

    private void AdjustArea()
    {
        int laymask = 1 << LayerMask.NameToLayer("Ground");
        for (int i = 0; i < areas.Length; ++i)
        {
            var Hit = Physics2D.BoxCast(
                areas[i].transform.position, 
                Vector2.one, 0f, Vector2.zero, 1f, laymask);
        }
    }
} // class TilemapSC