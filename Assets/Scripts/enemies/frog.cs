using UnityEngine;

public class Frog : BaseEnemy
{
    [Header("Player Detection")]
    public Transform player;
    public float detectionRange = 3f;
    public float attackCooldown = 2f;

    private float lastAttackTime;
    private Animator animator;
    private bool playerInRange;
    private Vector2 direction;
    private bool ShouldFlip = false;
    private bool isFlipped = false;
    public HealthManager healthManager;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            if (player != null)
            {
                if (player.IsSpinning())
                {
                    player.Bounce();
                }

                else
                {

                        player.Die();
                    
                    
                }
            }
        }

        else if (collision.gameObject.CompareTag("Rock"))
        {
            base.Die();
        }
    }

    protected override void Update()
    {
        base.Update();
        if (StopGame.Instance != null && StopGame.Instance.IsFrozen())
        {
            rb.linearVelocity = Vector2.zero;
            return; // skip rest of logic
        }
        if (player == null) return;
        direction = (player.transform.position - transform.position).normalized;
        HandleFlipping();

        if (playerInRange && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    public override void Attack()
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
            Debug.Log("fuck you ");
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

    private void HandleFlipping()
    {
        if (direction.x > 0 && !isFlipped)
        {
            ShouldFlip = true;
            Flip();
        }
        else if (direction.x < 0 && isFlipped)
        {
            ShouldFlip = true;
            Flip();
        }
    }
    private void Flip()
    {
        if (ShouldFlip)
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
            isFlipped = !isFlipped;
            ShouldFlip = false;
        }
    }

    protected override void Move() { }

}
