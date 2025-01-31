using UnityEngine;
using UnityEngine.UI;

public class ExitButtonSC : MonoBehaviour
{
    public int enabledSecond = 120;
    private Button m_ExitButton;
    private Color m_NormalColor;

    private void Awake()
    {
        m_ExitButton = GetComponent<Button>();
    }

    private void Start()
    {
        GameManagerSC.Instance.AddTimeEvent(enabledSecond, EnabledButton);
    }

    public void EnabledButton()
    {
        m_ExitButton.enabled = false;
        var colorTint = m_ExitButton.colors;
        colorTint = m_ExitButton.colors;
    }

} // class ExitButtonSC
