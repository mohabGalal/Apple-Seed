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

    public bool LiquidPicked;

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
        


    }

    private void HandleMovementInput()
    {
        float InputX = Input.GetAxisRaw("Horizontal");
        // rb.linearVelocityX = InputX * RunningSpeed;
        rb.linearVelocity = new Vector2(InputX * RunningSpeed, rb.linearVelocity.y);

        isMoving = InputX != 0;
        

        if (Input.GetKeyDown(KeyCode.Space) && !LiquidPicked)
        {
            if (isGrounded)
                Jump();
            else
                DoubleJump();

        
            anim.SetTrigger("isJumping");
        }

        if (Input.GetKeyDown(KeyCode.Space) && LiquidPicked)
        {
            SpinJump();
            LiquidPicked = false;
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
        anim.SetFloat("Yvelocity", rb.linearVelocityY);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity);
        isGrounded = false;
    }

    private void DoubleJump()
    {
        if (heartsCollected > 0 && canDoubleJump)
        {
            anim.SetFloat("Yvelocity", rb.linearVelocityY);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity);
            heartsCollected--;
            canDoubleJump = false;
        }

    }

    private void SpinJump()
    {
        anim.SetTrigger("SpinJump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity* 1.6f);
        isGrounded = false;
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

        // Calculate spawn position relative to player
        Vector3 spawnPosition = transform.position;
        float handOffset = isFacingRight ? 1f : -1f; 
        spawnPosition.x += handOffset;
        spawnPosition.y += 0.5f; 

        GameObject rock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rockRb = rock.GetComponent<Rigidbody2D>();
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        rockRb.AddForce(direction * throwForce, ForceMode2D.Impulse);
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
