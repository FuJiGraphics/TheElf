using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EmptyAreaSC : MonoBehaviour
{
    public bool HasGround { get; set; } = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EmptyArea")
        {
            EmptyAreaSC sc = other.gameObject.GetComponent<EmptyAreaSC>();
            if (sc.HasGround)
            {
                return;
            }
            else
            {

            }
        }
    }

} // class EmptyAreaSC
