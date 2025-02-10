using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChoicePanelUI : MonoBehaviour
{
    public ButtonUI backButton;
    public ButtonUI homeButton;

    private ButtonUI[] m_Buttons;
    private Animator m_Animator;

    private void OnEnable()
    {
        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }
        m_Animator.enabled = true;
        m_Animator.SetTrigger("Enable");
    }

    private void Start()
    {
        this.Init();
    }

    private void Init()
    {
        m_Buttons = GetComponentsInChildren<ButtonUI>();
        foreach (var button in m_Buttons)
        {
            if (button.name == "Button_Back")
            {
                backButton = button;
                backButton.button.onClick.AddListener(this.OnClickBack);
            }
            else if (button.name == "Button_Home")
            {
                homeButton = button;
                homeButton.button.onClick.AddListener(this.OnClickHome);
            }
        }
        m_Animator = GetComponent<Animator>();
    }

    private void OnClickBack()
    {
        this.gameObject.SetActive(false);
    }

    private void OnClickHome()
    {
        this.gameObject.SetActive(false);
    }

} // class StageChoicePanelUI
