using UnityEngine;

public class PowerUpLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit PowerUp");
            gameObject.SetActive(false);
        }

    }

    
}
