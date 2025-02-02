using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveupSC : MonoBehaviour
{
    public Button[] buttons;

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(OnClickYesButton);
        buttons[1].onClick.AddListener(OnClickNoButton);
    }

    private void OnClickYesButton()
    {
        GameManagerSC.Instance.DefeatGame();
    }

    private void OnClickNoButton()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameManagerSC.Instance.PauseGame();
    }

    private void OnDisable()
    {
        GameManagerSC.Instance.StartGame();
    }

} // class GiveupSC
