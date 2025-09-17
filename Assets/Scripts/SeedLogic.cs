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
            // Disable this seed visuals
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = null;

            ++seedsCollected;
            if (seedsCollected % 2 == 0)
            {
                MovingPlatform script = movingPlatform.GetComponent<MovingPlatform>();
                script.ChangeTarget();
            }

            // Notify player
            PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SeedCollected();
            }

            // Count seed
            
            Debug.Log($"Seeds collected: {seedsCollected}");

            // Update UI
            if (seedsCollected - 1 < seedImages.Count)
            {
                Color c = seedImages[seedsCollected - 1].color;
                c.a = 1;
                seedImages[seedsCollected - 1].color = c;
            }

            // Update trees + win
            StartCoroutine(SetTreeActive());
        }
    }

    public static void ResetSeedCount()
    {
        seedsCollected = 0;
        Debug.Log($"seeds reset {seedsCollected}");
    }

    IEnumerator SetTreeActive()
    {
        Debug.Log("Seed collected, changing tree...");
        yield return new WaitForSeconds(1);

        // Hide all trees
        foreach (var tree in trees)
        {
            tree.SetActive(false);
        }

        // Show correct tree
        if (seedsCollected - 1 < trees.Count)
        {
            trees[seedsCollected - 1].SetActive(true);
        }

        // Check win condition
        if (seedsCollected >= seedImages.Count) // all seeds collected
        {
            yield return new WaitForSeconds(0.5f);
            WinScreen.SetActive(true);
            SoundManager.Instance.PlayWinScreen();
        }

        Destroy(gameObject);
    }
}
