using UnityEditor;
using UnityEngine;
 
public abstract class BaseEnemy : MonoBehaviour
{
    public float health = 100f;
    public float speed = 2f;
    public int DirectionX = 1;

    public GameObject ExplosionPreFab;

    protected Rigidbody2D rb;
    protected Transform player;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void flip()
    {
       // DirectionX = - DirectionX;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    protected abstract void Move();
    public abstract void Attack();

    public virtual void Die()
    {
        Destroy(gameObject);
        GameObject Explosion = Instantiate(ExplosionPreFab, transform.position, Quaternion.identity);
       // Destroy(Explosion);

    }
}