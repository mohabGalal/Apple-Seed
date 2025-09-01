using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public Dictionary<string, GameObject> powerUps;
    public List<GameObject> powerUpPrefab; //Assign the PowerUp assets (prefabs) from the inspector
    public Transform PowerUpSpawner;
    public Transform StartingPoint;
    public Transform EndingPoint;


    void Start()
    {
        powerUps = new Dictionary<string, GameObject>();

        //just checkking
        for (int i = 0 ; i < powerUpPrefab.Count; i++)
        {
            Debug.Log(powerUpPrefab[i].name);
        }

        //Assign the prefab (the asset) to the "value" of the dictionary, and the name of it as the "key"
        for(int i = 0; i < powerUpPrefab.Count; i++) {

            powerUps.Add(powerUpPrefab[i].name, powerUpPrefab[i]);
        }

        //just checking again aghhhhh
        for (int i = 0; i < powerUps.Count; i++)
        {

            Debug.Log($"Power ups : {powerUps.Keys}");
        }

        Randomize();  // run the code multiple times you will see the pickups change randomly at the same position 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Randomize()
    {      
        int index = Random.Range(0, powerUps.Count);
        GameObject CurrentPowerUp = Instantiate(powerUpPrefab[index], PowerUpSpawner.position, Quaternion.identity); 
    }
}
