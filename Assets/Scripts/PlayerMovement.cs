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

    private Rigidbody2D rb;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
  
    void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        float InputX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocityX = InputX * RunningSpeed;

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocityY = JumpVelocity;
            isGrounded = false;
        }
    }

    private bool IsGrounded()
    {
       isGrounded =  Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer);
        return isGrounded;
    }
    private void Jump()
    {

    }

    private void DoubleJump()
    {

    }

    private void Flip()
    {

    }
}
