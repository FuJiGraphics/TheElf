using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnforcePanelSC : MonoBehaviour
{
    public struct EnforceItem
    {
        public Button Button { get; set; }
        public Image Image { get; set; }
        public TextMeshProUGUI Text { get; set; }
        public GameObject Prefab { get; set; }
    }

    public GameObject[] bows;
    public GameObject[] crossBows;
    public GameObject[] swords;
    public GameObject[] potions;

    private EnforceItem[] m_Slots;

    private void Awake()
    {
        this.Init();
    }

    private void Init()
    {
        m_Slots = new EnforceItem[3];
        var buttons = GetComponentsInChildren<Button>();
        if (buttons == null)
        {
            Debug.LogError("Did not found Slot Buttons!");
        }
        var slots = GetComponentsInChildren<SlotItemSC>();
        if (slots == null)
        {
            Debug.LogError("Did not found Sots Scripts!");
        }

        if (buttons.Length != slots.Length)
        {
            Debug.LogError("Mismatch between the number of slots and buttons!");
        }
        for (int i = 0; i < slots.Length; ++i)
        {
            m_Slots[i].Button = buttons[i];
            m_Slots[i].Image = slots[i].image;
            m_Slots[i].Text = slots[i].text;
        }
    }

} // class EnforcePanelSC
