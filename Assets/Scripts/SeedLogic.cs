using UnityEngine;

public class SeedLogic : MonoBehaviour
{
    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;

    private static int seedsCollected = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            PlayerMovement Player = collision.collider.GetComponent<PlayerMovement>();
            if (Player != null)
            {
                Player.SeedCollected();
            }
            ++seedsCollected;

            Debug.Log($"seeds collected {seedsCollected}");
            
            if(seedsCollected == 1)
            {
                tree1.SetActive(false);
                tree2.SetActive(true);
                tree3.SetActive(false);
            }

            if (seedsCollected == 2)
            {
                tree1.SetActive(false);
                tree2.SetActive(false);
                tree3.SetActive(true);
            }
        }
    }


}
