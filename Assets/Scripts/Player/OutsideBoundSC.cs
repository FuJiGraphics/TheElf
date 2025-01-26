using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideBoundSC : MonoBehaviour
{
    public enum BoundDirection
    {
        Top, Bottom, Left, Right,
    };

    public Grid targetGrid;
    public GameObject tilemapPrefab;
    public BoundDirection type;
    public MainCameraSC targetViewport;
    
    private Vector3 m_DirectionVec;
    public static Dictionary<Vector2Int, bool> s_ObjMap = 
        new Dictionary<Vector2Int, bool>();

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
                Vector2Int targetKey = Vector2Int.zero;
                targetKey.x = (int)nextPos.x;
                targetKey.y = (int)nextPos.y;
                if (!s_ObjMap.ContainsKey(targetKey))
                {
                    var go = GameObject.Instantiate(tilemapPrefab, targetGrid.transform);
                    go.transform.position = nextPos;
                    s_ObjMap.Add(targetKey, true);
                }
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
