using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButtonUI : ButtonUI
{
    public GameObject popupLogin;

    protected override void OnAwake()
    {
        button.onClick.AddListener(this.OnClick);
    }

    void OnClick()
    {
        popupLogin.SetActive(true);
    }

} // class LoginButtonUI
