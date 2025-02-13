using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupLoginUI : MonoBehaviour
{
    public TMP_InputField id;
    public TMP_InputField password;
    public Button loginButton;
    public Button signUpButton;
    public Button closeButton;
    public GameObject signUpPanel;

    private void Start()
    {
        loginButton.onClick.AddListener(this.OnClickLogin);
        signUpButton.onClick.AddListener(this.OnClickSignUp);
        closeButton.onClick.AddListener(this.OnClickCloseButton);
    }

    private void OnClickLogin()
    {
        if (GameManagerSC.Instance.LoginId(id.text, password.text))
        {
            this.OnSucceededLogin();
        }
        else
        {
            this.OnFailedLogin();
        }
    }

    private void OnClickSignUp()
    {
        signUpPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnClickCloseButton()
    {
        this.gameObject.SetActive(false);
    }

    private void OnSucceededLogin()
    {
        GameManagerSC.Instance.FadeOut(() => LoadSceneManager.LoadScene("LobbyScene"));
    }

    private void OnFailedLogin()
    {
        var msgGo = UtilManager.FindWithName("MessagePopup");
        MessagePopupUI msg = msgGo.GetComponent<MessagePopupUI>();
        msg.SetTitle("Login Failed");
        msg.SetText("Incorrect username or password. Please try again.");
        msg.gameObject.SetActive(true);
    }

} // class PopupLoginUI
