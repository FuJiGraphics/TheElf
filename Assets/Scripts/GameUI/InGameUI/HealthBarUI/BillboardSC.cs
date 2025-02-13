using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSC : MonoBehaviour
{
    public GameObject billboardCamera;

    private void Start()
    {
        billboardCamera = GameObject.FindWithTag("MainCamera");
    }

    private void LateUpdate()
    {
        if (billboardCamera == null)
            return;

        transform.LookAt(transform.position + billboardCamera.transform.forward);
    }

} // class BillboardSC
