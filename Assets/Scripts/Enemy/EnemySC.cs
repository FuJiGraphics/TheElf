using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySC : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 5f;
    public int attackPower = 10;
    public int health = 100;

    private bool isCollided = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!isCollided)
        {
            this.Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollided = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollided = false;
        }
    }

    public virtual void Move()
    {
        if (target == null)
            return;

        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.Normalize();
        targetDir.z = 0f;

        Vector3 velocity = targetDir * moveSpeed * Time.deltaTime;
        transform.position += velocity;
    }
}
