using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class EnforcePanelSC : MonoBehaviour
{
    public enum SlotType
    {
        LevelupWeapons,
        UnusedDefaultWeapons,
        Potions,
        NoMoreItems,
    }

    public struct EnforceItem
    {
        public GameObject LevelUpPrev { get; set; }
        public GameObject LevelUpNext { get; set; }
        public WeaponSC SelectRandomDefaultWeapon { get; set; }
        public Button Button { get; set; }
        public Image Image { get; set; }
        public Image Background { get; set; }
        public TextMeshProUGUI Text { get; set; }
        public GameObject Prefab { get; set; }
        public SlotItemSC SlotItem { get; set; }
    }

    public Sprite[] slotLevelImage;
    public TextMeshProUGUI levelTitle;
    public Color emptySlotColor;

    private EnforceItem[] m_Slots;
    private PlayerSC m_TargetPlayer;

    private readonly int m_SlotCount = 3;

    private void Awake()
    {
        this.Init();
    }

    private void OnEnable()
    {
        GameManagerSC.Instance.PauseGame();
        this.AdjustSlots();
    }

    public void SetTargetPlayer(PlayerSC player)
    {
        if (LogManager.IsVaild(player))
        {
            m_TargetPlayer = player;
        }
        else
        {
            m_TargetPlayer = null;
        }
    }

    private void Init()
    {
        var mainPlayerGo = GameObject.FindWithTag("Player");
        if (LogManager.IsVaild(mainPlayerGo))
        {
            m_TargetPlayer = mainPlayerGo.GetComponent<PlayerSC>();
        }
        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in texts)
        {
            if (text.gameObject.name == "Text_LevelCounter")
            {
                levelTitle = text;
            }
        }

        m_Slots = new EnforceItem[m_SlotCount];
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
            slots[i].Init();
            m_Slots[i].Button = buttons[i];
            m_Slots[i].Image = slots[i].image;
            m_Slots[i].Text = slots[i].text;
            m_Slots[i].Background = slots[i].background;
            m_Slots[i].SlotItem = slots[i];
        }
        m_Slots[0].Button.onClick.AddListener(OnSlot0);
        m_Slots[1].Button.onClick.AddListener(OnSlot1);
        m_Slots[2].Button.onClick.AddListener(OnSlot2);
    }

    private void AdjustSlots()
    {
        int iterIndex = 0;
        this.levelTitle.text = m_TargetPlayer.Level.ToString();
        iterIndex = this.SelectedLevelUpWeapons(iterIndex);
        iterIndex = this.SelectedRandomWeapon(iterIndex);
        this.SelectedDisableEmptySlots(iterIndex);
    }

    private int SelectedLevelUpWeapons(int iterIndex = 0)
    {
        var allWeapons = m_TargetPlayer.allWeapons;

        if (allWeapons != null && allWeapons.Count > 0)
        {
            List<int> weaponIndices = Enumerable.Range(0, allWeapons.Count).ToList();
            weaponIndices = weaponIndices.OrderBy(x => UnityEngine.Random.value).ToList(); // 리스트 셔플
            for (int i = 0; i < Math.Min(m_SlotCount, weaponIndices.Count); i++)
            {
                if (iterIndex >= m_SlotCount)
                {
                    break;
                }
                int sel = weaponIndices[i];
                if (allWeapons[sel].activeSelf)
                {
                    GameObject nextWeapon = WeaponManager.Instance.LevelUp(allWeapons[sel]);
                    if (LogManager.IsVaild(nextWeapon))
                    {
                        this.SetSlotItem(iterIndex, allWeapons[sel], SlotType.LevelupWeapons, nextWeapon);
                        iterIndex++;
                    }
                }
            }
        }
        return iterIndex;
    }

    private int SelectedRandomWeapon(int iterIndex = 0)
    {
        List<WeaponSC> unused = m_TargetPlayer.GetUnusedWeapons();
        if (unused != null && unused.Count > 0)
        {
            List<int> weaponIndices = Enumerable.Range(0, unused.Count).ToList();
            weaponIndices = weaponIndices.OrderBy(x => UnityEngine.Random.value).ToList();

            for (int i = 0; i < Math.Min(m_SlotCount, weaponIndices.Count); i++)
            {
                int sel = weaponIndices[i];
                if (iterIndex < m_SlotCount)
                {
                    this.SetSlotItem(iterIndex, unused[sel].gameObject, SlotType.UnusedDefaultWeapons);
                    iterIndex++;
                }
            }
        }
        return iterIndex;
    }
    
    private void SelectedDisableEmptySlots(int iterIndex = 0)
    {
        if (iterIndex < m_SlotCount)
        {
            for (int i = iterIndex; i < m_Slots.Length; ++i)
            {
                m_Slots[i].Button.interactable = false;
                m_Slots[i].Image.color = Color.clear;
            }
        }
    }

    private void SetSlotItem(int slotIndex, GameObject weapon, SlotType type, GameObject levelUpTarget = null)
    {
        if (LogManager.IsNull(weapon))
            return;

        if (SlotType.LevelupWeapons == type)
        {
            if (LogManager.IsVaild(levelUpTarget))
            {
                WeaponSC nextWeaponSC = levelUpTarget.GetComponent<WeaponSC>();
                m_Slots[slotIndex].Image.sprite = nextWeaponSC.icon;
                m_Slots[slotIndex].Text.text = nextWeaponSC.desc;
                m_Slots[slotIndex].SelectRandomDefaultWeapon = null;
                m_Slots[slotIndex].LevelUpNext = levelUpTarget;
                m_Slots[slotIndex].LevelUpPrev = weapon;
                int slotBackgroundIndex = Math.Clamp(nextWeaponSC.level - 1, 0, slotLevelImage.Length);
                m_Slots[slotIndex].Background.sprite = slotLevelImage[nextWeaponSC.level - 1];
                m_Slots[slotIndex].SlotItem.SetStar(nextWeaponSC.level);
            }
        }
        else if (SlotType.UnusedDefaultWeapons == type)
        {
            WeaponSC originWeapon = weapon.GetComponent<WeaponSC>();
            m_Slots[slotIndex].Image.sprite = originWeapon.icon;
            m_Slots[slotIndex].Text.text = originWeapon.desc;
            m_Slots[slotIndex].SelectRandomDefaultWeapon = originWeapon;
            m_Slots[slotIndex].LevelUpNext = null;
            m_Slots[slotIndex].LevelUpPrev = null;
            int slotBackgroundIndex = Math.Clamp(originWeapon.level - 1, 0, slotLevelImage.Length);
            m_Slots[slotIndex].Background.sprite = slotLevelImage[originWeapon.level - 1];
            m_Slots[slotIndex].SlotItem.SetStar(originWeapon.level);
        }
        else if (SlotType.Potions == type)
        {
            // 미구현
            m_Slots[slotIndex].Button.onClick.RemoveAllListeners();
        }
        else
        {
            m_Slots[slotIndex].Button.onClick.RemoveAllListeners();
        }
    }

    private void OnSlot0() => OnSlot(0);
    private void OnSlot1() => OnSlot(1);
    private void OnSlot2() => OnSlot(2);

    private void OnSlot(int index)
    {
        if (LogManager.IsNull(m_TargetPlayer))
            return;

        if (m_Slots[index].SelectRandomDefaultWeapon != null)
        {
            m_Slots[index].SelectRandomDefaultWeapon.gameObject.SetActive(true);
        }
        else if (m_Slots[index].LevelUpNext != null)
        {
            GameObject prevItem = m_Slots[index].LevelUpPrev;
            GameObject nextItem = m_Slots[index].LevelUpNext;
            m_TargetPlayer.ChangeItem(prevItem, nextItem);
        }
        this.gameObject.SetActive(false);
        GameManagerSC.Instance.StartGame();
    }

} // class EnforcePanelSC
