using UnityEngine;

public class SpinJump : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement Player = collision.GetComponent<PlayerMovement>();
            if (Player != null)
            {
                Player.LiquidPicked = true;
                Player.hasSpinPower = true;
            }
        }
    }
}
