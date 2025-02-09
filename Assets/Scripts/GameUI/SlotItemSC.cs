using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemSC : MonoBehaviour
{
    public Image image;
    public Image background;
    public TextMeshProUGUI text;

    private bool m_IsInitailized = false;

    private void Awake()
    {
        this.Init();
    }

    public void Init()
    {
        if (m_IsInitailized)
            return;
        m_IsInitailized = true;

        var images = GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            if (image.gameObject.name == "Slot")
            {
                this.background = image;
            }
            if (image.gameObject.name == "Item")
            {
                this.image = image;
                break;
            }
        }
        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in texts)
        {
            if (text.gameObject.name == "Index")
            {
                this.text = text;
                break;
            }
        }
    }

} // class SlotItemSC
