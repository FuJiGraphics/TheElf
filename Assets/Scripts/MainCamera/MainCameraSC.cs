using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraSC : MonoBehaviour
{
    public Rect rect;

    private Camera m_Camera;
    private BoxCollider2D m_BoxCollider;
    private float m_PrevOrthoSize;

    private void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_BoxCollider = GetComponent<BoxCollider2D>();
        this.AdjustColliderSize();
    }

    private void FixedUpdate()
    {
        this.AdjustColliderSize();
    }

    private void AdjustColliderSize()
    {
        if (m_PrevOrthoSize == m_Camera.orthographicSize)
            return;

        float boxHeight = 2f * m_Camera.orthographicSize;
        float boxWidth = boxHeight * ((float)m_Camera.pixelWidth / (float)m_Camera.pixelHeight);
        m_BoxCollider.size = new Vector2(boxWidth * 2f, boxHeight * 2f);
    }
}
