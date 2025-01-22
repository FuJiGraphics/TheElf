using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManagerSC : Singleton<TilemapManagerSC>
{
    public Tilemap[] tilemaps;

    private static Queue<Tilemap> m_Unused;
    private static List<Tilemap> m_Used;
    private static bool m_IsInitialized = false;

    private void Start()
    {
        this.Init();
    }

    public Tilemap Get()
    {
        this.Init();
        if (m_Unused.Count < 1)
            return null;

        Tilemap unusedTilemap = m_Unused.Dequeue();
        m_Used.Add(unusedTilemap);
        return unusedTilemap;
    }

    public void Return(Tilemap tilemap)
    {
        this.Init();

        if (m_Used.Contains(tilemap) == false)
            return;

        for (int i = 0; i < m_Used.Count; ++i)
        {
            if (m_Used[i] == tilemap)
            {
                m_Unused.Enqueue(m_Used[i]);
                m_Used.Remove(m_Used[i]);
                break;
            }
        }
    }

    private void Init()
    {
        if (m_IsInitialized)
            return;
        m_Unused = new Queue<Tilemap>();
        m_Used = new List<Tilemap>();
        for (int i = 0; i < tilemaps.Length; ++i)
        {
            m_Used.Add(tilemaps[i]);
        }
        m_IsInitialized = true;
    }
} // class TilemapManagerSC
