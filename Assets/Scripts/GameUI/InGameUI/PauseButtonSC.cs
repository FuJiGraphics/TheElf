using UnityEngine;
using UnityEngine.UI;

public class PauseButtonSC : MonoBehaviour
{
    public GameObject activeObject;
    public Button pauseButton;

    private void Start()
    {
        pauseButton = GetComponentInChildren<Button>();
        pauseButton.onClick.AddListener(OnClickPauseButton);
    }

    private void OnClickPauseButton()
    {
        activeObject.SetActive(true);
    }

} // class PauseButtonSC
