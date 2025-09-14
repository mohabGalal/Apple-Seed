using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SpawnRange
{
    public Transform StartPos;
    public Transform EndPos;
}

public class EagleSpawner : MonoBehaviour
{
    public GameObject EaglePreFab;
    [SerializeField] public List<SpawnRange> SpawnPositions;

    [Header("Drop Points")]
    public Transform[] dropPoints;
    void Start()
    {
       
        SpawnEagle();

    }

    private void SpawnEagle()
    {
        for (int i = 0; i < SpawnPositions.Count; i++)
        {
            float startPos = SpawnPositions[i].StartPos.position.x;
            float endPos = SpawnPositions[i].EndPos.position.x;
            float newPos = Random.Range(startPos, endPos);
            Vector3 RandomPlace = new Vector3(newPos, SpawnPositions[i].StartPos.position.y, SpawnPositions[i].StartPos.position.z);

            GameObject newEagle = Instantiate(EaglePreFab, RandomPlace, Quaternion.identity);

            // Assign drop point to the instantiated eagle
            Eagle eagleScript = newEagle.GetComponent<Eagle>();
            if (eagleScript != null && i < dropPoints.Length)
            {
                eagleScript.dropPoint = dropPoints[i];
            }
        }
    }
}
