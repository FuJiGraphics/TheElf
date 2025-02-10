using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanelUI : MonoBehaviour
{
    public ButtonUI settingButton;
    public ButtonUI StageChoiceButton;
    public ButtonUI UpgradeStatButton;

    private ButtonUI[] m_Buttons;
    private GameObject m_Panel;

    private void Awake()
    {
        this.Init();
    }

    private void Init()
    {
        m_Panel = UtilManager.FindWithName("StageChoicePanel");

        m_Buttons = GetComponentsInChildren<ButtonUI>();
        foreach (var button in m_Buttons)
        {
            if (button.name == "LobbySettingUI")
            {
                settingButton = button;
                settingButton.button = settingButton.GetComponent<Button>();
                settingButton.button.onClick.AddListener(this.OnClickSettingButton);
            }
            else if (button.name == "StageChoiceUI")
            {
                StageChoiceButton = button;
                StageChoiceButton.button = StageChoiceButton.GetComponent<Button>();
                StageChoiceButton.button.onClick.AddListener(this.OnClickStageChoiceButton);
            }
            else if (button.name == "StageUpgradeStat")
            {
                UpgradeStatButton = button;
                UpgradeStatButton.button = UpgradeStatButton.GetComponent<Button>();
                UpgradeStatButton.button.onClick.AddListener(this.OnClickUpgradeStatButton);
            }
        }
    }

    private void OnClickSettingButton()
    {

    }

    private void OnClickStageChoiceButton()
    {
        m_Panel.SetActive(true);
    }

    private void OnClickUpgradeStatButton()
    {

    }

} // class LobbyPanelUI
