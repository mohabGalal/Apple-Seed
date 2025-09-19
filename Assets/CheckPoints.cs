using UnityEngine;

public class CheckPoints : MonoBehaviour
{

    public Transform CheckPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")){

            PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.transform.position = CheckPoint.position;
                player.Die();
            }
        }
    }
}
