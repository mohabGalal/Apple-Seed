using UnityEngine;

public class Key : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            Debug.Log("Collision with player");
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.handlePowerUps("Key");
                player.IsFrozen = true;
                gameObject.SetActive(false);
            }
        }
    }

}
