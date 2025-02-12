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

    private void Start()
    {
        loginButton.onClick.AddListener(this.OnClickLogin);
    }

    private void OnClickLogin()
    {
        int loginId = int.Parse(id.text);
        if (GameManagerSC.Instance.LoginId(loginId))
        {
            Debug.Log("�α��� ����!");
            LoadSceneManager.LoadScene("LobbyScene");
        }
        else
        {
            Debug.Log("�α��� ����!");
        }
    }

} // class PopupLoginUI
