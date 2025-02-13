using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemSC : MonoBehaviour
{
    public Image image;
    public Image background;
    public GameObject[] stars;
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

    public void SetStar(int star)
    {
        star = Mathf.Clamp(star, 1, stars.Length);
        star = star - 1;

        for (int i = 0; i < stars.Length; ++i)
        {
            stars[i].SetActive(false);
        }
        stars[star].SetActive(true);
    }

} // class SlotItemSC
