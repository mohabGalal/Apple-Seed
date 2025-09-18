using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Setup")]
    public List<GameObject> enemies; // Assign enemy prefabs in Inspector
    public Transform spawnStart;     // Left/Start boundary
    public Transform spawnEnd;       // Right/End boundary

    [Header("Spawning Rules")]
    public int numberOfEnemies = 10;
    public float minDistance = 6f;   // Minimum distance between enemies
    private float snailLength = 8f;

    Vector3 newPos = Vector3.zero;

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (enemies == null || enemies.Count == 0)
        {
            Debug.LogWarning("Enemy list is empty! Assign prefabs in the Inspector.");
            return;
        }

        // Build prefab index list (balanced distribution)
        List<int> prefabIndexes = new List<int>();
        int baseCount = numberOfEnemies / enemies.Count;
        int remainder = numberOfEnemies % enemies.Count;

        for (int i = 0; i < enemies.Count; i++)
        {
            int count = baseCount + (i < remainder ? 1 : 0);
            for (int j = 0; j < count; j++)
            {
                prefabIndexes.Add(i);
            }
        }

        // Shuffle the index list
        for (int i = 0; i < prefabIndexes.Count; i++)
        {
            int rand = Random.Range(i, prefabIndexes.Count);
            int temp = prefabIndexes[i];
            prefabIndexes[i] = prefabIndexes[rand];
            prefabIndexes[rand] = temp;
        }

        // Spawn enemies
        int attempts = 0;
        int maxAttempts = numberOfEnemies * 10;
        int indexCounter = 0;

        while (spawnedPositions.Count < numberOfEnemies && attempts < maxAttempts)
        {
            attempts++;

            float randX = Random.Range(spawnStart.position.x, spawnEnd.position.x);
            float fixedY = spawnStart.position.y; // Use start’s Y so they’re aligned

            newPos = new Vector3(randX, fixedY, 0f);
            indexCounter = Random.Range(0, enemies.Count);

            if (enemies[indexCounter].CompareTag("Snail"))
            {
                if(isOutOfBounds())
                {

                }
            }

            bool isClose = false;
            foreach (var pos in spawnedPositions)
            {
                if (Vector3.Distance(newPos, pos) < minDistance)
                {
                    isClose = true;
                    break;
                }
            }

            if (!isClose)
            {
                spawnedPositions.Add(newPos);
                
                Instantiate(enemies[indexCounter], new Vector2(newPos.x, newPos.y + enemies[indexCounter].transform.position.y), Quaternion.identity);
            }
        }

        Debug.Log($"Spawned {spawnedPositions.Count} enemies successfully!");
    }


    bool isOutOfBounds()
    {
        if(Vector2.Distance(newPos , spawnEnd.position) < 4)
        {
            return true;
        }
        return false;
    }
}
