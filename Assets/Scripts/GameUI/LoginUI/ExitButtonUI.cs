using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonUI : ButtonUI
{
    protected override void OnClickButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

} // class ExitButtonUI
