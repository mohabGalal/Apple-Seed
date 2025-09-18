using UnityEngine;

public class HeartHealth : MonoBehaviour
{
    public HealthManager healthManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            healthManager.RestoreHeats();
            Debug.Log("Heart taken");
            Destroy(gameObject);
           
        }
    }
}
