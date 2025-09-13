using UnityEngine;

public class Eagle : BaseEnemy
{
    public float swoopSpeed = 5f;     
    private bool hasTarget = false;
    public Transform PlayerPos;
    private Vector2 direction;
    private bool ShouldFlip = false;
    private bool isFlipped = false;
    public Transform StartPos;
    private PlayerMovement Player;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
             Player = collision.collider.GetComponent<PlayerMovement>();
            if (Player != null)
            {
                Player.Die();
                Destroy(gameObject, 1);
            }
        }
    }

    override protected void Awake()
    {
        base.Awake();
        PlayerPos = Player.transform;
        
    }

    override protected void Update()
    {
        base.Update();

        if (player != null)
        {
             direction = (PlayerPos.position - transform.position).normalized;
            Debug.Log($"Direstion : {direction.x} , {direction.y}");
            rb.linearVelocity = direction * swoopSpeed;
            if(direction.x > 0  && !isFlipped)
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
    }

    protected override void Move()
    {
    }

    public override void Attack()
    {
   
        Debug.Log("Eagle attacks the apple!");
    }

    private void Flip()
    {

        if (ShouldFlip )
        {
            
            Debug.Log("Player is on the left");
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
           
            isFlipped = !isFlipped;
        }

        
      
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
