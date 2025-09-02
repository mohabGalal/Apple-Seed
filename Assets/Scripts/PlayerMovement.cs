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

    public int extraJumpsValue = 1; 
    private int extraJumps;        


    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpsValue; 

    }
  
    void Update()
    {
        isGrounded = IsGrounded();

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        float InputX = Input.GetAxisRaw("Horizontal");
        // rb.linearVelocityX = InputX * RunningSpeed;
        rb.linearVelocity = new Vector2(InputX * RunningSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
                Jump();
            else
                DoubleJump();
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
       isGrounded =  Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer);
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
        if (extraJumps > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity);
            --extraJumps;
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
}
