using System.Xml.Serialization;
using UnityEngine;

public class Snail : BaseEnemy
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerMovement Player = collision.collider.GetComponent<PlayerMovement>();
           
            if (Player != null) {
                
                if (Player.IsSpinning())
                {
                    //Die();
                   // Enemy dies
                  //  Destroy(gameObject);
                    Player.Bounce(); 
                }
                else
                {
                    Player.Die(); 
                }

            }
        }

        if (collision.collider.CompareTag("StartPoint") || collision.collider.CompareTag("EndPoint"))
        {
            DirectionX = - DirectionX;
            flip();
        }

        if (collision.collider.CompareTag("Rock"))
        {
            Die();
        }
    }

    

    override protected void Awake()
    {
        base.Awake();
    }

    override protected void Update()
    {

        base.Update();
        if (StopGame.Instance != null && StopGame.Instance.IsFrozen())
        {
            // Stop movement
            rb.linearVelocity = Vector2.zero;
            return; // skip rest of logic
        }
    }

    override public void Die()
    {
        base.Die();
        Debug.Log($"snail Killed at {gameObject.transform.position}");
    }

    protected override void Move()
    {
        // rb.linearVelocityX = speed * DirectionX; 
        rb.linearVelocity = new Vector2(speed * DirectionX, rb.linearVelocity.y);
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
