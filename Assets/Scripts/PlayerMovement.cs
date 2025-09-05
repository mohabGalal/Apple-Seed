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

    public void Die()
    {
       // Destroy(gameObject);
        Debug.Log("Player Dies");
    }
}
