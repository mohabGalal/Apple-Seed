using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    //  public Dictionary<string, GameObject> powerUps;
    public List<GameObject> powerUpPrefab; //Assign the PowerUp assets (prefabs) from the inspector
    public Transform PowerUpSpawner;
    public Transform StartingPoint;
    public Transform EndingPoint;

    public int numberOfPowerUps = 10 ;
    float MinDistance = 6f;
    private List<Vector3> spawnedPositions = new List<Vector3>();




    void Start()
    {
        // powerUps = new Dictionary<string, GameObject>();

        //just checkking
        for (int i = 0; i < powerUpPrefab.Count; i++)
        {
            Debug.Log(powerUpPrefab[i].name);
        }


        SpawnPowerUps();  
    }

    // Update is called once per frame
    void Update()
    {

    }

   

    private void SpawnPowerUps()
    {
        // build  prefab index  list

        List<int> prefabIndexes = new List<int>();
        int baseCount = numberOfPowerUps / powerUpPrefab.Count;
        int remainder = numberOfPowerUps % powerUpPrefab.Count;

        for (int i = 0; i < powerUpPrefab.Count; i++)
        {
            int count = baseCount + (i < remainder ? 1 : 0);
            for (int j = 0; j < count; j++)
            {
                prefabIndexes.Add(i);
            }
        }

        // shuffle the list 
        for (int i = 0; i < prefabIndexes.Count; i++)
        {
            int rand = Random.Range(i, prefabIndexes.Count);
            int temp = prefabIndexes[i];
            prefabIndexes[i] = prefabIndexes[rand];
            prefabIndexes[rand] = temp;
        }

        //spawn with spacing check

        int attempts = 0;
        int maxAttempts = numberOfPowerUps * 10;
        int indexCounter = 0;

        while (spawnedPositions.Count < numberOfPowerUps && attempts < maxAttempts)
        {
            attempts++;


            float RandX = Random.Range(StartingPoint.position.x, EndingPoint.position.x);
            float RandY = StartingPoint.position.y;

            Vector3 newPos = new Vector3(RandX, RandY, 0f);


            bool isClose = false;

            foreach (var pos in spawnedPositions)
            {
                if (Vector3.Distance(newPos, pos) < MinDistance)
                {
                    isClose = true;
                    break;
                }
            }
            if (!isClose)
            {
                // this pos  is valied so we can add it 
                spawnedPositions.Add(newPos);

                // pick a random prefab
                int prefabIndex = prefabIndexes[indexCounter++];
                Instantiate(powerUpPrefab[prefabIndex], newPos, Quaternion.identity);
            }

        }
    }
}
