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


    void Start()
    {
       // powerUps = new Dictionary<string, GameObject>();

        //just checkking
        for (int i = 0 ; i < powerUpPrefab.Count; i++)
        {
            Debug.Log(powerUpPrefab[i].name);
        }


        Randomize();  // run the code multiple times you will see the pickups change randomly at the same position 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Randomize()
    {      
        int index = Random.Range(0, powerUpPrefab.Count);
        GameObject CurrentPowerUp = Instantiate(powerUpPrefab[index], PowerUpSpawner.position, Quaternion.identity); 
    }
}
