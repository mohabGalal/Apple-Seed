using UnityEngine;

public class DeathArea : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerMovement Player = collision.collider.GetComponent<PlayerMovement>();
            if (Player != null)
            {
                Player.FinalDeath();
            }
        }
    }
}
