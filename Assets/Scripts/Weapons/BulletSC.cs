using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSC : MonoBehaviour
{
    public int attackPower = 20;
    public float survivalTime = 5f;

    float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= survivalTime)
        {
            this.Destroy();
            elapsedTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var sc = collision.gameObject.GetComponent<IDefender>();
            sc.TakeDamage(attackPower);
            if (sc.IsDie)
            {
                // 킬 카운트 증가
                GameManagerSC.Instance.KillCount++;
            }
        }
    }

    private void Destroy()
    {
        GameObject.Destroy(gameObject);
    }

} // class BulletSC
