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

    private void OnEnable()
    {
        GameManagerSC.Instance.PauseGame();
    }

    private void OnClickYesButton()
    {
        gameObject.SetActive(false);
        GameManagerSC.Instance.DefeatGame();
    }

    private void OnClickNoButton()
    {
        gameObject.SetActive(false);
        GameManagerSC.Instance.StartGame();
    }

} // class GiveupSC
