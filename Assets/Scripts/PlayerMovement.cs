using System.Security.Cryptography.X509Certificates;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float RunningSpeed = 8f;
    private float JumpVelocity = 12f;
    private bool isFacingRight = true;
    private bool isGrounded = true;
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    private float GroundRadius = 0.2f;
    private int heartsCollected = 0;
    private bool canDoubleJump = true;
    public Animator anim;
    bool isMoving;
    public bool CanThrowRock;
   // public Transform handPoint;      
    public GameObject rockPrefab;    
    public float throwForce = 10f;
    public Transform handPointt;


    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        AnimationController();
        isGrounded = IsGrounded();

        if (isGrounded)
        {
            canDoubleJump = true;
        }
        HandleMovementInput();

        if( (Input.GetKeyDown(KeyCode.T)) && CanThrowRock)
        {
            throwRock();
        }

        Debug.Log($"can throw : {CanThrowRock}");
    }

    private void AnimationController()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetFloat("Yvelocity", rb.linearVelocityY);


    }

    private void HandleMovementInput()
    {
        float InputX = Input.GetAxisRaw("Horizontal");
        // rb.linearVelocityX = InputX * RunningSpeed;
        rb.linearVelocity = new Vector2(InputX * RunningSpeed, rb.linearVelocity.y);

        isMoving = InputX != 0;
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
                Jump();
            else
                DoubleJump();

        
            anim.SetTrigger("isJumping");
        }

        if (isFacingRight && InputX < 0)
        {
            Flip();
        }
        else if (!isFacingRight && InputX > 0)
        {
            Flip();
        }


    }

    private bool IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer);
        return isGrounded;
    }
    private void Jump()
    {
        // rb.linearVelocityY = JumpVelocity;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity);
        isGrounded = false;
    }

    private void DoubleJump()
    {
        if (heartsCollected > 0 && canDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity);
            heartsCollected--;
            canDoubleJump = false;
        }

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

        // transform.rotation = Quaternion.Euler(0,isFacingRight ? 180 : 0,0);
    }

    public void AddHeart(int amount)
    {
        heartsCollected += amount;
    }

    public void throwRock()
    {
        anim.SetTrigger("Throw");
        CanThrowRock = false;
        GameObject rock = Instantiate(rockPrefab, handPointt.position, Quaternion.identity);
        Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();

        if(rock != null)
        {
            Debug.Log($"Rock Position : { rock.gameObject.transform.position}");
        }

        // Determine throw direction 
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        Debug.Log("Inside the throw function ");

    }
    public void canThrowRock()
    {
        CanThrowRock = true;
    }
    public void Die()
    {
       // Destroy(gameObject);
        Debug.Log("Player Dies");
        anim.SetBool("isDead", true);
       
    }
}
