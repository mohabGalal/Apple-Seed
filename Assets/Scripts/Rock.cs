using UnityEngine;

public class Rock : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            
            PlayerMovement Player = collision.collider.GetComponent<PlayerMovement>();
            if (Player != null)
            {
                Player.CanThrowRock = true;
                Debug.Log("Setting the CanThrow value");
            }

            Destroy(gameObject);
        }
    }
}
