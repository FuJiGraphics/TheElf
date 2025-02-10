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
    }

    protected virtual void Start()
    {

    }

} // class class BackButtonUI
