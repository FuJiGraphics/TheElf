using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideBoundSC : MonoBehaviour
{
    public enum BoundDirection
    {
        Top, Bottom, Left, Right,
    };

    public BoundDirection type;
    public MainCameraSC targetViewport;
    public TilemapManagerSC tilemapManager;
    
    private Vector3 m_DirectionVec;

    private void Start()
    {
        m_DirectionVec = this.GetDirectionVec();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tilemap"))
        {
            var tilemaps = targetViewport.CurrenTilemap;
            for (int i = 0; i < tilemaps.Count; ++i)
            {
                Vector3 nextPos = tilemaps[i].transform.position + m_DirectionVec;
                tilemapManager.Get(nextPos);
            }
        }
    }

    private Vector3 GetDirectionVec()
    {
        switch (type)
        {
            case BoundDirection.Top:
                return Vector3.up * 33f;
            case BoundDirection.Bottom:
                return Vector3.down * 33f;
            case BoundDirection.Left:
                return Vector3.left * 33f;
            case BoundDirection.Right:
                return Vector3.right * 33f;
        }
        return Vector3.zero;
    }

} // class OutsideBoundSC
