using UnityEngine;

public class HeartHealth : MonoBehaviour
{
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            HealthManager.instance.RestoreHeats();
            Debug.Log("Heart taken");
            Destroy(gameObject);
           
        }
    }
}
