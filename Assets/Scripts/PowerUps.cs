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

    Vector3 newPos = Vector3.zero;
    float MinDistance = 6f;
    



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
        float RandX = Random.Range(StartingPoint.position.x, EndingPoint.position.x);
        float RandY = Random.Range(StartingPoint.position.y, EndingPoint.position.y);
        newPos.x = RandX;
        newPos.y = RandY;
        

        int index = Random.Range(0, powerUpPrefab.Count);
        GameObject CurrentPowerUp = Instantiate(powerUpPrefab[index], newPos, Quaternion.identity); 
    }
}
