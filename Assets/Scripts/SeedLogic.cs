using UnityEngine;
using UnityEngine.UI;

public class SeedLogic : MonoBehaviour
{
    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;

    public GameObject WinScreen;

    public Image seed1;
    public Image seed2;
    //public Image seed3;



    public static int seedsCollected = 0;
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
                Color c = seed1.color;
                c.a = 1;
                seed1.color = c;
            }

            if (seedsCollected == 2)
            {
                tree1.SetActive(false);
                tree2.SetActive(false);
                tree3.SetActive(true);
                Color c = seed2.color;
                c.a = 1;
                seed2.color = c;
                WinScreen.SetActive(true);
            }
        }
    }

    public static void ResetSeedCount()
    {
        seedsCollected = 0;
    }


}
