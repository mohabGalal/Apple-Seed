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

        int attempts = 0;
        int maxAttempts = numberOfPowerUps * 10;

        while (spawnedPositions.Count < numberOfPowerUps && attempts < maxAttempts)
        {
            attempts++;


            float RandX = Random.Range(StartingPoint.position.x, EndingPoint.position.x);
            float RandY = Random.Range(StartingPoint.position.y, EndingPoint.position.y);

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

                int idx = Random.Range(0, powerUpPrefab.Count);
                Instantiate(powerUpPrefab[idx], newPos, Quaternion.identity);
            }

        }
    }
}
