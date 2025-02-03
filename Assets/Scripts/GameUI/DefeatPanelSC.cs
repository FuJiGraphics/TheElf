using UnityEngine;
using UnityEngine.UI;

public class DefeatPanelSC : MonoBehaviour
{
    public Button[] buttons;

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(OnClickPlayAgainButton);
        buttons[1].onClick.AddListener(OnClickContinueButton);
    }

    private void OnClickPlayAgainButton()
    {
        GameManagerSC.Instance.RestartGame();
    }

    private void OnClickContinueButton()
    {
        GameManagerSC.Instance.RestartGame();
    }

    private void OnEnable()
    {
        GameManagerSC.Instance.PauseGame();
    }

} // class DefeatPanelSC
