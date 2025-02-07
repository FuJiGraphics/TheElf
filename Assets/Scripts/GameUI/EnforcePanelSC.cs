using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class EnforcePanelSC : MonoBehaviour
{
    public struct EnforceItem
    {
        public int TargetIndex { get; set; }
        public GameObject LevelUp { get; set; }
        public WeaponSC Selectable { get; set; }
        public Button Button { get; set; }
        public Image Image { get; set; }
        public TextMeshProUGUI Text { get; set; }
        public GameObject Prefab { get; set; }
    }

    public TextMeshProUGUI levelTitle;
    public GameObject[] potions;

    private EnforceItem[] m_Slots;
    private PlayerSC m_TargetPlayer;
    private List<WeaponSC> m_CurrentWeapons;
    private List<WeaponSC> m_UnusedWeapons;

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
            slots[i].Init();
            m_Slots[i].Button = buttons[i];
            m_Slots[i].Image = slots[i].image;
            m_Slots[i].Text = slots[i].text;
        }
        m_Slots[0].Button.onClick.AddListener(OnSlot0);
        m_Slots[1].Button.onClick.AddListener(OnSlot1);
        m_Slots[2].Button.onClick.AddListener(OnSlot2);
    }

    private void AdjustSlots()
    {
        var weaponSC = m_TargetPlayer.allWeapons[0].GetComponent<WeaponSC>();
        m_CurrentWeapons = m_TargetPlayer.allWeapons
            .Select(w => w.GetComponent<WeaponSC>()).ToList();

        bool hasUnusedWeapon = false;
        int iterIndex = 0;
        for (int i = 0; i < 3; ++i)
        {
            if (m_CurrentWeapons[i].gameObject.activeSelf)
            {
                // 레벨업
                GameObject nextWeapon = WeaponManager.LevelUp(m_CurrentWeapons[i]);
                if (LogManager.IsVaild(nextWeapon))
                {
                    this.ShowWeaponSlot(iterIndex, nextWeapon.GetComponent<WeaponSC>());
                    m_Slots[iterIndex].LevelUp = nextWeapon;
                    m_Slots[iterIndex].TargetIndex = i;
                    iterIndex++;
                }
                else
                {
                    // 흑백 처리 선택불가 또는 다른 아이템
                    // iterIndex++;
                    // this.ShowWeaponSlot(iterIndex, m_CurrentWeapons[i]);
                }
            }
            else
            {
                hasUnusedWeapon = true;
            }
        }

        if (hasUnusedWeapon)
        {
            m_UnusedWeapons = this.GetUnusedWeapons(ref m_CurrentWeapons);
            for (int i = 0; i < m_UnusedWeapons.Count; ++i)
            {
                m_Slots[iterIndex].Selectable = m_UnusedWeapons[i];
                this.ShowRandomWeaponSlot(iterIndex);
            }
        }
    }

    private void ShowRandomWeaponSlot(int slot)
    {
        if (m_UnusedWeapons == null || m_UnusedWeapons.Count < 1)
            return;
        int ran = Random.Range(0, m_UnusedWeapons.Count);
        this.ShowWeaponSlot(slot, m_UnusedWeapons[ran]);
        m_UnusedWeapons.Remove(m_UnusedWeapons[ran]);
    }

    private void ShowWeaponSlot(int slot, WeaponSC weapon)
    {
        m_Slots[slot].Image.sprite = weapon.icon;
        m_Slots[slot].Text.text = weapon.desc;
    }

    private List<WeaponSC> GetUnusedWeapons(ref List<WeaponSC> weapons)
    {
        List<WeaponSC> unusedWeapons = new List<WeaponSC>();
        foreach (WeaponSC weapon in weapons)
        {
            if (!weapon.gameObject.activeSelf)
            {
                unusedWeapons.Add(weapon);
            }
        }
        if (unusedWeapons.Count <= 0)
        {
            unusedWeapons = null;
        }
        return unusedWeapons;
    }

    private void OnSlot0()
    {
        if (LogManager.IsNull(m_TargetPlayer))
            return;

        if (m_Slots[0].Selectable != null)
        {
            m_Slots[0].Selectable.gameObject.SetActive(true);
        }

        if (m_Slots[0].LevelUp != null)
        {
            int targetIndex = m_Slots[0].TargetIndex;
            m_TargetPlayer.SetItem(targetIndex, m_Slots[0].LevelUp);
            m_CurrentWeapons = m_TargetPlayer.allWeapons
            .Select(w => w.GetComponent<WeaponSC>()).ToList();
        }
        this.gameObject.SetActive(false);
        GameManagerSC.Instance.StartGame();
    }

    private void OnSlot1()
    {
        if (LogManager.IsNull(m_TargetPlayer))
            return;

        if (m_Slots[1].Selectable != null)
        {
            m_Slots[1].Selectable.gameObject.SetActive(true);
        }
        if (m_Slots[1].LevelUp != null)
        {
            int targetIndex = m_Slots[1].TargetIndex;
            m_TargetPlayer.SetItem(targetIndex, m_Slots[1].LevelUp);
            m_CurrentWeapons = m_TargetPlayer.allWeapons
            .Select(w => w.GetComponent<WeaponSC>()).ToList();
        }
        this.gameObject.SetActive(false);
        GameManagerSC.Instance.StartGame();
    }

    private void OnSlot2()
    {
        if (LogManager.IsNull(m_TargetPlayer))
            return;

        if (m_Slots[2].Selectable != null)
        {
            m_Slots[2].Selectable.gameObject.SetActive(true);
        }
        if (m_Slots[2].LevelUp != null)
        {
            int targetIndex = m_Slots[2].TargetIndex;
            m_TargetPlayer.SetItem(targetIndex, m_Slots[2].LevelUp);
            m_CurrentWeapons = m_TargetPlayer.allWeapons
            .Select(w => w.GetComponent<WeaponSC>()).ToList();
        }
        this.gameObject.SetActive(false);
        GameManagerSC.Instance.StartGame();
    }

} // class EnforcePanelSC
