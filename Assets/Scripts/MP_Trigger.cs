using UnityEngine;

public class MP_Trigger : MonoBehaviour
{
    public MovingPlatform movingPlatform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        movingPlatform.enabled = true;
        movingPlatform.isMovingToStart = true;
        this.enabled = false;

    }
}
