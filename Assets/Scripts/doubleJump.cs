using UnityEngine;

// ya mohaaaaaaaaaaaaaaaaaaaaab 
public class DoubleJump : MonoBehaviour
{
    public int heartValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.AddHeart(heartValue);
                Debug.Log("Collected a Heart ");
            }

            Destroy(gameObject);
        }
    }
}
