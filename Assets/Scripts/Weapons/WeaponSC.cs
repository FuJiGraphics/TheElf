using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WeaponSC : MonoBehaviour
{
    public float attackPower = 20f;
    public float attackSpeed = 1f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    private float elapsedTime = 0f;

    public virtual void Shoot(Vector2 dir, Vector2 firePoint)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= attackSpeed)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint, Quaternion.identity);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = dir * projectileSpeed;
            }

            elapsedTime = 0f;
        }
    }
} // class PlayerWeaponSC
