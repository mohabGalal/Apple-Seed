using Unity.VisualScripting;
using UnityEngine;

public class EagleSpawn : MonoBehaviour
{
    public GameObject EaglePrefab;
    public GameObject EagleAura;
    public Transform eagleSpawner;
    public Transform DropPoint;

    public bool KeyTaken = false;


    private void Update()
    {
        if (KeyTaken)
        {
            InstantiateAura();
            KeyTaken = false;
        }
    }


    private void InstantiateAura()
    {
        Debug.Log("In Aura");
        GameObject Aura = Instantiate(EagleAura, eagleSpawner.position, Quaternion.identity);
        Destroy(Aura, 1.3f);
        StartCoroutine(instantiateEagle());
    }

    System.Collections.IEnumerator instantiateEagle()
    {
        yield return new WaitForSeconds(1.2f);

        GameObject eagle = Instantiate(EaglePrefab, eagleSpawner.position, Quaternion.identity);
        FriendEagle script = eagle.GetComponent<FriendEagle>();
        script.dropPoint = DropPoint;
    }

}
