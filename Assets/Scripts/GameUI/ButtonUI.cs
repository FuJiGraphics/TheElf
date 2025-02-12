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

} // class class BackButtonUI
