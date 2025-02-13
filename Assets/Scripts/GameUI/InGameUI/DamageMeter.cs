using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageMeter : MonoBehaviour
{
    static Dictionary<int, string> s_StringTable
        = new Dictionary<int, string>();

    public ObjectManagerSC ownerPool;
    private TextMeshProUGUI m_TextMeshPro;
    private Animator m_Aniamtor;

    private void Awake()
    {
        this.Init();
    }

    public void Play(string text)
    {
        this.Init();
        m_TextMeshPro.text = text;
        m_Aniamtor.SetTrigger("Play");
    }

    public void Play(int text)
    {
        this.Init();
        if (s_StringTable.ContainsKey(text))
        {
            m_TextMeshPro.text = s_StringTable[text];
        }
        else
        {
            s_StringTable.Add(text, text.ToString());
        }
        m_Aniamtor.SetTrigger("Play");
    }

    public void Init()
    {
        if (m_TextMeshPro == null)
        {
            m_TextMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        }
        if (m_Aniamtor == null)
        {
            m_Aniamtor = GetComponent<Animator>();
        }
        if (s_StringTable.Count <= 0)
        {
            for (int i = 0; i < 50000; ++i)
            {
                s_StringTable.Add(i, i.ToString());
            }
        }
    }

    public void OnAnimationEnd()
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (ownerPool)
        {
            ownerPool.Release(this.gameObject);
        }
        else
        {
            Debug.LogWarning("DamageMeter: ownerPool is not assigned. Destroying object instead.");
            Destroy(gameObject);
        }
    }
}
