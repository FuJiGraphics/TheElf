using TMPro;
using UnityEngine;

public class DamageMeter : MonoBehaviour
{
    public ObjectManagerSC ownerPool;
    private TextMeshProUGUI m_TextMeshPro;
    private Animator m_Aniamtor;
    private bool m_IsInitialized = false;

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
