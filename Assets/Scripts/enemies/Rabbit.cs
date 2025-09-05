using System.Xml.Serialization;
using UnityEngine;

public class Rabbit : BaseEnemy
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerMovement Player = collision.collider.GetComponent<PlayerMovement>();
            if (Player != null) {
                Player.Die();
            
            }
        }

        if (collision.collider.CompareTag("StartPoint") || collision.collider.CompareTag("EndPoint"))
        {
            DirectionX = - DirectionX;
        }
    }

    

    override protected void Awake()
    {
        base.Awake();
    }

    override protected void Update()
    {
        base.Update();
    }

    override public void Die()
    {
        base.Die();
        Debug.Log("Rabbit Killed");
    }

    protected override void Move()
    {
        rb.linearVelocityX = speed * DirectionX; 
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
