using UnityEngine;

public class Frog : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float detectionRange = 3f;
    public float attackCooldown = 2f;

    private float lastAttackTime;
    private Animator animator;
    private bool playerInRange;   

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().Die();
        }
    }


    void Update()
    {
        if (player == null) return;

        if (playerInRange && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");
    }

    public void TryHitPlayer()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            Debug.Log("Player got caught by frog!");
            player.GetComponent<PlayerMovement>().Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.transform; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }
}
