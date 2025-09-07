using System.Security.Cryptography.X509Certificates;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float RunningSpeed = 8f;
    private float JumpVelocity = 10f;
    private bool isFacingRight = true;
    private bool isGrounded = true;
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    private float GroundRadius = 0.2f;
    //private int heartsCollected = 0;
    public bool canDoubleJump = true;
    public Animator anim;
    bool isMoving;
    public bool CanThrowRock;
    public bool CanPickRock;
   // public Transform handPoint;      
    public GameObject rockPrefab;    
    public float throwForce = 10f;
    public Transform handPointt;
    public bool doubleJumpUnlocked = false;

    public Vector3 PlayerPos;
    public Transform PlayerSpawnPoint;

    public bool LiquidPicked;

    private Rigidbody2D rb;

    [SerializeField] public bool hasSpinPower = false; 
    [SerializeField] private float spinDelay = 0.8f;    // small delay after jump before spin can start
    [SerializeField] private float spinBounceVelocity = 10f;
    [SerializeField] private float spinGravityScale = 1.2f; 
    private float defaultGravityScale;

    private bool isSpinning = false;
    private float jumpStartTime = -999f;

    public bool IsSpinning() => isSpinning;
    public bool IsFalling() => rb.linearVelocity.y < -0.05f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;
    }

    void Update()
    {
       

        AnimationController();
        isGrounded = IsGrounded();

        if (isGrounded)
        {
            canDoubleJump = doubleJumpUnlocked;
            // End spin when touching the ground
            if (isSpinning)
                EndSpin();
        }

        HandleMovementInput();

        if ((Input.GetKeyDown(KeyCode.T)) && CanThrowRock)
        {
            throwRock();
        }

        
        if (!isGrounded && hasSpinPower && Input.GetKey(KeyCode.Space) && !isSpinning)
        {
            if (Time.time - jumpStartTime >= spinDelay)
                StartSpin();
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
        anim.SetFloat("Yvelocity", rb.linearVelocityY);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity);
        isGrounded = false;
        jumpStartTime = Time.time;   // mark jump start
    }


    private void DoubleJump()
    {
        /* if (heartsCollected > 0 && canDoubleJump)
         {
             anim.SetFloat("Yvelocity", rb.linearVelocityY);
             rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity);
             heartsCollected--;
             canDoubleJump = false;
         }*/
        if (canDoubleJump)
        {
            anim.SetFloat("Yvelocity", rb.linearVelocityY);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity);
            canDoubleJump = false;
        }

    }

    private void SpinJump()
    {
        anim.SetTrigger("SpinJump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity* 1.3f);
        isGrounded = false;
    }

    private void StartSpin()
    {
        isSpinning = true;
        anim.SetBool("isSpinning", true);  
        rb.gravityScale = spinGravityScale;
    }

    private void EndSpin()
    {
        isSpinning = false;
        //hasSpinPower = false;
        anim.SetBool("isSpinning", false);
        rb.gravityScale = defaultGravityScale;
    }

    
    public void Bounce()
    {
        if (!isSpinning && hasSpinPower && Input.GetKey(KeyCode.Space))
            StartSpin();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, spinBounceVelocity);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

        // transform.rotation = Quaternion.Euler(0,isFacingRight ? 180 : 0,0);
    }

  /*  public void AddHeart(int amount)
    {
        heartsCollected += amount;
    }*/

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
        handlePowerUps("Throw");
    }
    public void Die()
    {
       // Destroy(gameObject);
        Debug.Log("Player Dies");
        anim.SetBool("isDead", true);
       
    }

    public void handlePowerUps(string powerUpName)
    {
        switch (powerUpName)
        {
            case "DoubleJump":
                {
                    CanThrowRock = false;
                    hasSpinPower = false;
                    CanPickRock = false;
                    break;
                }
            case "Throw":
                {
                    canDoubleJump = false;
                    hasSpinPower = false;
                    doubleJumpUnlocked = false;
                    break;

                }
            case "SpinJump":
                {
                    CanThrowRock = false;
                    canDoubleJump = false;
                    CanPickRock = false;
                    doubleJumpUnlocked = false;
                    break;
                }
        }

    }

    public void SeedCollected()
    {
        transform.position = new Vector3 (PlayerSpawnPoint.position.x, PlayerSpawnPoint.position.y) ;
    }


}
