using UnityEngine;

public class SpinJump : MonoBehaviour
{
    private static int itemsCollected = 0;
    public GameObject SpinScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            itemsCollected++;

            if (itemsCollected == 1)
            {
                if (SpinScreen)
                {
                    SpinScreen.SetActive(true);
                    Destroy(SpinScreen, 5f);
                }
            }

            PlayerMovement Player = collision.GetComponent<PlayerMovement>();
            if (Player != null)
            {
                Player.LiquidPicked = true;
                Player.hasSpinPower = true;
                Player.handlePowerUps("SpinJump");
            }
        }
    }

    private void OnDestroy()
    {
        itemsCollected = 0;
    }
    private System.Collections.IEnumerator HandleScreen()
    {
        SpinScreen.SetActive(true);
        float waitTime = 5f;

        yield return new WaitForSeconds(waitTime);
        SpinScreen.SetActive(false);
    }
}
