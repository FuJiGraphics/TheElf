using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSlotButtonUI : ButtonUI
{
    public int stage = 1;
    public Animator animator;
    private GameObject m_LockGo;

    public bool IsLock { get; private set; } = false;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        button.onClick.AddListener(this.OnClickButton);

        var childs = GetComponentsInChildren<Transform>(true);
        foreach (var child in childs)
        {
            if (child.name == "Image_Lock")
            {
                m_LockGo = child.gameObject;
                IsLock = child.gameObject.activeSelf ? true : false;
            }
        }
    }

    public void SetLock(bool enabled)
    {
        IsLock = enabled;
        if (enabled)
        {
            m_LockGo.SetActive(true);
        }
        else
        {
            m_LockGo.SetActive(false);
        }
    }

    protected override void OnClickButton()
    {
        if (IsLock)
            return;

        animator.SetTrigger("OnClick");
        this.StartStage();
    }

    private void StartStage()
     => StartCoroutine(this.StartStageCoroutine());

    private IEnumerator StartStageCoroutine()
    {
        yield return new WaitForSeconds(1);
        GameManagerSC.Instance.FadeOut(() => LoadSceneManager.LoadScene("InGameScene"));
    }

} // class StageSlotButtonUI 
