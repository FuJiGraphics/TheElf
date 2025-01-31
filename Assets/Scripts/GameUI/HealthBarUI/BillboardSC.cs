using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSC : MonoBehaviour
{
    public Transform camera;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + camera.forward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + camera.forward);
    }
    
} // class BillboardSC
