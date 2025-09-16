using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    private static int KeyCount = 0;
    public GameObject KeyScreen;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ++KeyCount;
            if (KeyCount == 1)
            {
     /*           KeyScreen.SetActive(true);
                Destroy(KeyScreen, 5f);*/
            }
            EagleSpawn eagle = gameObject.GetComponentInParent<EagleSpawn>();
            eagle.KeyTaken = true;
            Debug.Log("Collision with player");
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.handlePowerUps("Key");
                player.IsFrozen = true;
                //gameObject.SetActive(false);
            }

            
        }

        

    }



}