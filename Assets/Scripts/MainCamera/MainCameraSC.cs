using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraSC : MonoBehaviour
{
    public BoxCollider2D[] outsideBounds;
    public List<GameObject> CurrenTilemap { get; private set; }

    private Camera m_Camera;
    private BoxCollider2D m_BoxCollider;

    private void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_BoxCollider = GetComponent<BoxCollider2D>();
        if (outsideBounds.Length < 4)
        {
            Debug.LogError("MainCamera의 Outside의 개수가 부족합니다. Left, Right, Top, Bottom이 있어야 합니다.");
        }
        CurrenTilemap = new List<GameObject>();
        this.AdjustColliderSize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tilemap"))
        {
            if (!CurrenTilemap.Contains(collision.gameObject))
            {
                CurrenTilemap.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tilemap"))
        {
            if (CurrenTilemap.Contains(collision.gameObject))
            {
                CurrenTilemap.Remove(collision.gameObject);
            }
        }
    }

    private void AdjustColliderSize()
    {
        float sizeW = m_BoxCollider.size.x;
        float sizeH = m_BoxCollider.size.y;
        m_BoxCollider.size = new Vector2(sizeW, sizeH);
        outsideBounds[0].offset = new Vector2(0f, sizeH * 0.75f);  // Top
        outsideBounds[1].offset = new Vector2(0f, sizeH * -0.75f); // Bot
        outsideBounds[2].offset = new Vector2(sizeW * -0.75f, 0f); // Left
        outsideBounds[3].offset = new Vector2(sizeW * 0.75f, 0f);  // Right
        outsideBounds[0].size = new Vector2(sizeW, sizeH * 0.5f);
        outsideBounds[1].size = new Vector2(sizeW, sizeH * 0.5f);
        outsideBounds[2].size = new Vector2(sizeW * 0.5f, sizeH);
        outsideBounds[3].size = new Vector2(sizeW * 0.5f, sizeH);
    }
}
