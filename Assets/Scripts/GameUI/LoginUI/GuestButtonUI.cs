using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestButtonUI : ButtonUI
{
    protected override void OnClickButton()
    {
        var game = GameManagerSC.Instance;
        string id = game.guestId;
        string pw = game.guestPassword;
        if (GameManagerSC.Instance.LoginId(id, pw))
        {
            GameManagerSC.Instance.FadeOut(() => LoadSceneManager.LoadScene("LobbyScene"));
        }
        else
        {
            this.OnFailedLoginGuest();
        }
    }

    private void OnFailedLoginGuest()
    {
        var msgGo = UtilManager.FindWithName("MessagePopup");
        MessagePopupUI msg = msgGo.GetComponent<MessagePopupUI>();
        msg.SetTitle("Guest Login Failed");
        msg.SetText("Incorrect username or password. Please try again.");
        msg.gameObject.SetActive(true);
    }

} // class GuestButtonUI
