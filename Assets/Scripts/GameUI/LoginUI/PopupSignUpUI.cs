using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSignUpUI : MonoBehaviour
{
    public TMP_InputField id;
    public TMP_InputField userName;
    public TMP_InputField password;
    public Button signUpButton;
    public Button loginButton;
    public Button closeButton;
    public GameObject loginPanel;

    private void Start()
    {
        signUpButton.onClick.AddListener(this.OnClickSignUp);
        loginButton.onClick.AddListener(this.OnClickLogin);
        closeButton.onClick.AddListener(this.OnCloseButton);
    }

    private void OnClickSignUp()
    {
        SaveData newUserData = new SaveData();
        newUserData.Id = id.text;
        newUserData.Password = password.text;
        newUserData.Name = userName.text;
        SignUpState result = GameManagerSC.Instance.SignUp(newUserData);
        this.Validation(result);
    }

    private void OnClickLogin()
    {
        loginPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnCloseButton()
    {
        this.gameObject?.SetActive(false);
    }

    private void Validation(SignUpState state)
    {
        switch (state)
        {
            case SignUpState.Done:
                this.OnClickLogin();
                break;
            case SignUpState.ExistsID:
                this.OnExistsID();
                break;
            case SignUpState.UnexpectedError:
                this.OnUnexpectedError();
                break;
            case SignUpState.LeastID:
                this.OnLeastID();
                break;
            case SignUpState.EmptyID:
                this.OnEmptyID();
                break;
            case SignUpState.LeastPassword:
                this.OnLeastPassword();
                break;
            case SignUpState.EmptyPassword:
                this.OnEmptyPassword();
                break;
            case SignUpState.EmptyUsername:
                this.OnEmptyUserName();
                break;
        }
    }

    private void OnExistsID()
    {
        var msgGo = UtilManager.FindWithName("MessagePopup");
        MessagePopupUI msg = msgGo.GetComponent<MessagePopupUI>();
        msg.SetTitle("Sign Up Failed");
        msg.SetText("Oops! That ID is already registered.");
        msg.gameObject.SetActive(true);
    }

    private void OnUnexpectedError()
    {
        var msgGo = UtilManager.FindWithName("MessagePopup");
        MessagePopupUI msg = msgGo.GetComponent<MessagePopupUI>();
        msg.SetTitle("Sign Up Failed");
        msg.SetText("Unexpected Error.");
        msg.gameObject.SetActive(true);
    }

    private void OnLeastID()
    {
        var msgGo = UtilManager.FindWithName("MessagePopup");
        MessagePopupUI msg = msgGo.GetComponent<MessagePopupUI>();
        msg.SetTitle("Sign Up Failed");
        msg.SetText("Your ID must be at least 6 characters long.");
        msg.gameObject.SetActive(true);
    }

    private void OnEmptyID()
    {
        var msgGo = UtilManager.FindWithName("MessagePopup");
        MessagePopupUI msg = msgGo.GetComponent<MessagePopupUI>();
        msg.SetTitle("Sign Up Failed");
        msg.SetText("Oops! You forgot to enter your ID.");
        msg.gameObject.SetActive(true);
    }

    private void OnLeastPassword()
    {
        var msgGo = UtilManager.FindWithName("MessagePopup");
        MessagePopupUI msg = msgGo.GetComponent<MessagePopupUI>();
        msg.SetTitle("Sign Up Failed");
        msg.SetText("Your Password must be at least 6 characters long.");
        msg.gameObject.SetActive(true);
    }

    private void OnEmptyPassword()
    {
        var msgGo = UtilManager.FindWithName("MessagePopup");
        MessagePopupUI msg = msgGo.GetComponent<MessagePopupUI>();
        msg.SetTitle("Sign Up Failed");
        msg.SetText("Oops! You forgot to enter your Password.");
        msg.gameObject.SetActive(true);
    }

    private void OnEmptyUserName()
    {
        var msgGo = UtilManager.FindWithName("MessagePopup");
        MessagePopupUI msg = msgGo.GetComponent<MessagePopupUI>();
        msg.SetTitle("Sign Up Failed");
        msg.SetText("Oops! You forgot to enter your Username.");
        msg.gameObject.SetActive(true);
    }

} // class PopupSignUpUI
