using UnityEngine;

public class EagleSpawner : MonoBehaviour
{

    public Transform StartPos;
    public Transform EndPos;
    public GameObject EaglePreFab;
    public Transform PlayerPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEagle();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEagle()
    {
        float newPos = Random.Range(StartPos.position.x, EndPos.position.x);
        Vector3 RandomPlace = new Vector3 (newPos , StartPos.position.y , StartPos.position.z);
        GameObject newEagle = Instantiate(EaglePreFab, RandomPlace, Quaternion.identity);
        Eagle script = newEagle.GetComponent<Eagle>();
        script.PlayerPos = PlayerPosition;

    }
}
