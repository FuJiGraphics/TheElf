using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    public Button button;

    protected virtual void OnEnable()
    {
        button = GetComponent<Button>();
    }
    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        OnAwake();
    }
    protected virtual void Start()
    {
        OnStart();
    }

    protected virtual void OnAwake()
    {
        // Empty
    }

    protected virtual void OnStart()
    {
        // Empty
    }

    private void OnClick()
    {
        OnClickButton();
    }

    protected virtual void OnClickButton()
    {
        // Empty
    }

} // class class BackButtonUI
