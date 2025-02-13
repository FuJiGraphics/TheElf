using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ClearButtonUI : MonoBehaviour
{
    public Button clearButton;

    private void Start()
    {
        clearButton.onClick.AddListener(this.OnClickClearButton);
    }

    private void OnClickClearButton()
    {
        GameManagerSC.Instance.ClearSaveData();
    }

} // class ClearButtonUI
