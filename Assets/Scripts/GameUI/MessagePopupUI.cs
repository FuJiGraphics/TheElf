using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopupUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(this.OnClickCloseButton);
    }

    private void OnClickCloseButton()
    {
        this.gameObject.SetActive(false);
    }

    public void SetTitle(string title)
    {
        titleText.text = title;
    }

    public void SetText(string text)
    {
        descText.text = text;
    }

} // class MeesagePopupUI
