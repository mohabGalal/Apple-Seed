using UnityEngine;

public class Eagle : BaseEnemy
{
    public float swoopSpeed = 5f;     
    private bool hasTarget = false;  

    override protected void Awake()
    {
        base.Awake();
    }

    override protected void Update()
    {
        base.Update();

        if (hasTarget && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * swoopSpeed;
        }
    }

    protected override void Move()
    {
    }

    public override void Attack()
    {
   
        Debug.Log("Eagle attacks the apple!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  
        {
            hasTarget = true;
            Attack();
        }
    }
}
