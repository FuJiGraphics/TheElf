using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSC : MonoBehaviour
{
    public GameObject camera;

    private void Start()
    {
        camera = GameObject.FindWithTag("MainCamera");
    }

    private void LateUpdate()
    {
        if (camera == null)
            return;

        transform.LookAt(transform.position + camera.transform.forward);
    }

} // class BillboardSC
