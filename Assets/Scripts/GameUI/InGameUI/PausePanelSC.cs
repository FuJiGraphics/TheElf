using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelSC : MonoBehaviour
{
    public Button continueButton;

    private void Start()
    {
        continueButton = GetComponentInChildren<Button>();
        continueButton.onClick.AddListener(OnClickContinueButton);
    }

    private void OnClickContinueButton()
    {
        GameManagerSC.Instance.StartGame();
        gameObject.SetActive(false);
    }

} // class PausePanelSC
