using UnityEngine;

public class Eagle : BaseEnemy
{
    [Header("Movement")]
    public float swoopSpeed = 5f;
    public float carrySpeed = 3f;
    public float activationDistance = 40f;

    [Header("Carry Behavior")]
    public Transform dropPoint;          // Where to drop the player
    public float liftHeight = 3f;        // How high to lift before traveling
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
        dropPoint = carriedPlayer.DropPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && currentState == EagleState.Hunting)
        {
            carriedPlayer = collision.collider.GetComponent<PlayerMovement>();
            if (carriedPlayer != null)
            {
                // Start carrying sequence
                currentState = EagleState.Lifting;
                carriedPlayer.ShouldFly = true;
                carriedPlayer.AppleAttachement = appleAttachement.transform;

                // Set lift target (current position + lift height)
                liftTarget = transform.position + Vector3.up * liftHeight;
                startCarryPosition = transform.position;
            }
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

    private void HandleHunting()
    {
        if (!ShouldBeActive())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Normal hunting behavior
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * swoopSpeed;
            HandleFlipping();
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
            currentState = EagleState.Carrying;
        }
    }

    private void HandleCarrying()
    {
        if (dropPoint == null)
        {
            Debug.LogWarning("No drop point set for eagle!");
            DropPlayer();
            return;
        }

        // Move toward drop point
        Vector2 carryDirection = (dropPoint.position - transform.position).normalized;
        rb.linearVelocity = carryDirection * carrySpeed;
        HandleFlipping();

        // Check if reached drop point
        if (Vector3.Distance(transform.position, dropPoint.position) < 1f)
        {
            currentState = EagleState.Dropping;
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