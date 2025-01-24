using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WeaponSC : MonoBehaviour
{
    public float attackSpeed = 1f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    private float elapsedTime = 0f;

    public virtual void Shoot(Vector2 dir, Vector2 firePoint)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= attackSpeed)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;

            elapsedTime = 0f;
        }
    }
} // class PlayerWeaponSC
