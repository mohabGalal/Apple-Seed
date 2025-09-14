using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerMovement Player = collision.collider.GetComponent<PlayerMovement>();
            if (Player != null )
            {
                Player.Die();
            }
        }
    }
}
