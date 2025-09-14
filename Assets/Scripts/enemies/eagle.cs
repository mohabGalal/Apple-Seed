using UnityEngine;

public class Eagle : BaseEnemy
{
    [Header("Movement")]
    public float swoopSpeed = 5f;
    public float carrySpeed = 3f;
    public float activationDistance = 50f;

    [Header("Carry Behavior")]
    public Transform dropPoint;          // Where to drop the player
    public float liftHeight = 1.5f;        // How high to lift before traveling
    public Transform appleAttachement;   // Where player attaches

    private Vector2 direction;
    private bool ShouldFlip = false;
    private bool isFlipped = false;
    private Camera mainCamera;
    private PlayerMovement carriedPlayer;

    // State management
    private enum EagleState { Hunting, Lifting, Carrying, Dropping }
    private EagleState currentState = EagleState.Hunting;
    private Vector3 liftTarget;
    private Vector3 startCarryPosition;

    override protected void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && currentState == EagleState.Hunting)
        {
            carriedPlayer = collision.collider.GetComponent<PlayerMovement>();
            if (carriedPlayer != null)
            {
                if (!carriedPlayer.HasKey)
                {
                    carriedPlayer.Die();
                    return;
                }

                currentState = EagleState.Lifting;
                carriedPlayer.ShouldFly = true;
                carriedPlayer.AppleAttachement = appleAttachement.transform;
                carriedPlayer.IsFrozen = false;
                liftTarget = transform.position + Vector3.up * liftHeight;
                startCarryPosition = transform.position;
            }
        }
        if (collision.collider.CompareTag("Rock"))
        {
            base.Die();
        }
    }



    private bool ShouldBeActive()
    {
        if (mainCamera == null) return false;
        float distanceFromCamera = Mathf.Abs(transform.position.x - mainCamera.transform.position.x);
        return distanceFromCamera < activationDistance;
    }

    override protected void Update()
    {
        base.Update();

        switch (currentState)
        {
            case EagleState.Hunting:
                HandleHunting();
                break;
            case EagleState.Lifting:
                HandleLifting();
                break;
            case EagleState.Carrying:
                HandleCarrying();
                break;
            case EagleState.Dropping:
                HandleDropping();
                break;
        }

        // Update player position if carrying
        if (carriedPlayer != null && carriedPlayer.ShouldFly)
        {
            carriedPlayer.FLyWithEagle();
        }
    }

    private bool isActivelyHunting = false;

    private void HandleHunting()
    {
        // Check if we should start hunting
        if (!isActivelyHunting)
        {
            if (!ShouldBeActive())
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            // Start hunting - from now on, don't check activation distance
            if (player != null)
            {
                isActivelyHunting = true;
            }
        }

        // Hunt the player
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * swoopSpeed;
            HandleFlipping();

           // Debug.Log($"Eagle hunting - distance to player: {Vector2.Distance(transform.position, player.position)}");
        }
    }

    private void HandleLifting()
    {
        // Move up to lift height
        Vector2 liftDirection = (liftTarget - transform.position).normalized;
        rb.linearVelocity = liftDirection * carrySpeed;

        // Check if reached lift height
        if (Vector3.Distance(transform.position, liftTarget) < 0.5f)
        {
            Debug.Log($"REached lift height : {Vector3.Distance(transform.position, liftTarget)}");
            currentState = EagleState.Carrying;
        }
    }

    private void HandleCarrying()
    {
        Debug.Log("Carrying the player away");
        if (dropPoint == null)
        {
            Debug.LogWarning("No drop point set for eagle!");
            DropPlayer();
            return;
        }

        Vector2 horizontalDirection = new Vector2(dropPoint.position.x - transform.position.x, 0).normalized;

        rb.linearVelocity = new Vector2(horizontalDirection.x * carrySpeed, -0.3f);

        if (horizontalDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -15f); // Slight downward tilt when moving right
        }
        else if (horizontalDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 15f);  // Slight downward tilt when moving left
        }

        // Custom flipping for horizontal carrying
        if (horizontalDirection.x > 0 && !isFlipped)
        {
            ShouldFlip = true;
            Flip();
        }
        else if (horizontalDirection.x < 0 && isFlipped)
        {
            ShouldFlip = true;
            Flip();
        }

        // Check if reached drop point horizontally
        float horizontalDistance = Mathf.Abs(dropPoint.position.x - transform.position.x);

        if (horizontalDistance < 2f)
        {
            currentState = EagleState.Dropping;
            Debug.Log("Reached drop point horizontally - dropping player");
        }
    }
    

    private void HandleDropping()
    {
        // Stop moving and drop player
        rb.linearVelocity = Vector2.zero;
        DropPlayer();

        // Optional: Destroy eagle or make it fly away
        Destroy(gameObject, 2f);
    }

    private void DropPlayer()
    {
        if (carriedPlayer != null)
        {
            carriedPlayer.ShouldFly = false;
            carriedPlayer.rb.gravityScale = carriedPlayer.originalGravityScale; // You'll need this in PlayerMovement
            carriedPlayer = null;
        }
    }

    private void HandleFlipping()
    {
        if (direction.x > 0 && !isFlipped)
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
  

    protected override void Move() { }
    public override void Attack() { }

    private void Flip()
    {
        if (ShouldFlip)
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
            isFlipped = !isFlipped;
            ShouldFlip = false;
        }
    }
}