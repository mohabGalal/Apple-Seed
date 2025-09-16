using UnityEngine;

public class StopGame : MonoBehaviour
{

    public static StopGame Instance { get; private set; }

    private bool isFrozen = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist between scenes
    }

    public void FreezeAllEnemies()
    {
        isFrozen = true;
    }

    public void UnfreezeAllEnemies()
    {
        isFrozen = false;
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }
}


