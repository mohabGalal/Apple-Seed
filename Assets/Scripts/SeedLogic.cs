using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedLogic : MonoBehaviour
{
    [Header("Tree progression")]
    public List<GameObject> trees;      
    public GameObject WinScreen;

    [Header("Platform")]
    public MovingPlatform movingPlatform;

    [Header("Seed Images (UI)")]
    public List<Image> seedImages;      

    public static int seedsCollected = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
          
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = null;

           
            MovingPlatform script = movingPlatform.GetComponent<MovingPlatform>();
            script.ChangeTarget();

            
            PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SeedCollected();
            }

           
            ++seedsCollected;
            Debug.Log($"Seeds collected: {seedsCollected}");

           
            if (seedsCollected - 1 < seedImages.Count)
            {
                Color c = seedImages[seedsCollected - 1].color;
                c.a = 1; 
                seedImages[seedsCollected - 1].color = c;
            }

            
            StartCoroutine(SetTreeActive());

        }
    }

    public static void ResetSeedCount()
    {
        seedsCollected = 0;
    }

    IEnumerator SetTreeActive()
    {
        Debug.Log("Seed collected, changing tree...");
        yield return new WaitForSeconds(1);

        
        foreach (var tree in trees)
        {
            tree.SetActive(false);
        }

        
        if (seedsCollected - 1 < trees.Count)
        {
            trees[seedsCollected].SetActive(true);
        }

    
        if (seedsCollected >= trees.Count)
        {
            yield return new WaitForSeconds(0.5f);
            WinScreen.SetActive(true);
            SoundManager.Instance.PlayWinScreen();
        }

        Destroy(gameObject);
    }
}
