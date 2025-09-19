using UnityEngine;

public class NormalMovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB; 
    public float speed = 2f;
    public float cooldownTime = 1f;

    private Vector3 target;
    private bool isWaiting = false; 
    private float waitTimer = 0f;

    void Start()
    {
        target = pointB.position; 
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= cooldownTime)
            {
                isWaiting = false;
                waitTimer = 0f;
            }
            return; 
        }

       
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            if (target == pointB.position)
                target = pointA.position;
            else
                target = pointB.position;

            isWaiting = true; 
        }
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}


