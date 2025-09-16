using UnityEngine;

public class fox : BaseEnemy
{
    public GameObject BulletPreFab;
    public Transform BulletSpawner;
    private Vector2 direction;
    private bool ShouldFlip = false;
    private bool isFlipped = false;
    public float throwForce = 15f;
    public float throwDistance = 10f;
    public float destroyTime = 2f;
    private bool CanAttack = true;
    public float attackCooldown = 3f;

    override protected void Update()
    {
        if (StopGame.Instance != null && StopGame.Instance.IsFrozen())
        {
            // Stop movement
            rb.linearVelocity = Vector2.zero;
            return; // skip rest of logic
        }
        direction = (player.transform.position - transform.position).normalized;
        HandleFlipping();
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < throwDistance && CanAttack)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        GameObject bullet = Instantiate(BulletPreFab, BulletSpawner.position, Quaternion.identity);
        Rigidbody2D BulletRb = bullet.GetComponent<Rigidbody2D>();
        BulletRb.AddForce(direction * throwForce, ForceMode2D.Impulse);

        // Start cooldown
        StartCoroutine(AttackCooldown());
        Destroy(bullet, destroyTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die();
            }
        }
        if (collision.collider.CompareTag("Rock"))
        {
            base.Die();
        }

    }
    private System.Collections.IEnumerator AttackCooldown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        CanAttack = true;
    }

    protected override void Move() { }

    private void HandleFlipping()
    {
        if (direction.x < 0 && !isFlipped)
        {
            ShouldFlip = true;
            Flip();
        }
        else if (direction.x > 0 && isFlipped)
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


}
