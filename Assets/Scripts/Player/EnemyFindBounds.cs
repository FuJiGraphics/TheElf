using UnityEngine;

public class EnemyFindBounds : MonoBehaviour
{
    public GameObject CurrentEnemy { get; private set; } = null;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.activeSelf)
        {
            CurrentEnemy = collision.gameObject;
        }
    }
    
} // class Bounds
