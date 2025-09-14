using UnityEngine;

public class fox : BaseEnemy
{
    public GameObject BulletPreFab;
    public Transform BulletSpawner;
    private Vector2 direction;
    private bool ShouldFlip = false;
    private bool isFlipped = false;
    private float throwForce = 15f;
    private bool CanAttack = true;
    public float attackCooldown = 3f;

    override protected void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        HandleFlipping();
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < 6 && CanAttack)
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
        Destroy(bullet, 1f);
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
