using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{
    public Image[] stars;

    private void Start()
    {
        stars = GetComponentsInChildren<Image>();
    }

    public void SetStar(bool enabled)
    {
        for (int i = 0; i < stars.Length; ++i)
        {
            stars[i].gameObject.SetActive(enabled);
        }
    }

} // class
