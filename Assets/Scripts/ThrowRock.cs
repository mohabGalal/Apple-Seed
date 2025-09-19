using UnityEngine;

public class ThrowRock : MonoBehaviour
{
    public GameObject ThrowScreen;

    private static int starCollected = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerMovement Player = collision.collider.GetComponent<PlayerMovement>();

            SoundManager.Instance.PlayPowerUp();

            starCollected++;
            if(starCollected == 1)
            {
                if (ThrowScreen)
                {
                    ThrowScreen.SetActive(true);
                    Destroy(ThrowScreen, 5f);
                }


            }

            if (Player != null)
            {
                Player.CanPickRock = true;
                Player.handlePowerUps("Throw");
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        starCollected = 0;
    }
}
