using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemSC : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

} // class SlotItemSC
