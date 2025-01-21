using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    private TouchManager touch;
    private Vector3 currDir;
    private Vector3 targetPos;

    private void Start()
    {
        touch = TouchManager.Instance;
        currDir = Vector2.right;
    }

    private void Update()
    {
        if (touch.TouchHeld.Active)
        {
            targetPos = Camera.main.ScreenToWorldPoint(touch.TouchHeld.Position);
            targetPos.z = 0f;
            currDir = (targetPos - transform.position).normalized;
            currDir.z = 0f;
        }

        Vector3 velocity = (currDir * moveSpeed * Time.deltaTime);
        if ((targetPos - transform.position).magnitude > velocity.magnitude)
        {
            transform.position += velocity;
        }
        else
        {
            transform.position = targetPos;
        }
    }

} // class PlayerMovement
