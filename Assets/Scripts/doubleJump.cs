using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class DoubleJump : MonoBehaviour
{
    public static int heartValue = 0;
    public GameObject DoubleScreen;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                heartValue++;
                if (heartValue == 1)
                {
                    DoubleScreen.SetActive(true);
                    Destroy(DoubleScreen, 5f);
                }
                //player.AddHeart(heartValue); 
                player.doubleJumpUnlocked = true;
                player.handlePowerUps("DoubleJump");
                Debug.Log("Collected a Heart ");
            }

            //Destroy(gameObject);
        }
    }
}
