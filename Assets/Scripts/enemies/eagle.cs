using Unity.VisualScripting;
using UnityEngine;

public class Eagle : BaseEnemy
{
    [Header("Movement")]
    public float swoopSpeed = 5f;
    public float carrySpeed = 6f;
    public float activationDistance = 50f;

    public Transform appleAttachement;   // Where player attaches

    private Vector2 direction;
    private bool ShouldFlip = false;
    private bool isFlipped = false;
    private Camera mainCamera;
    private PlayerMovement Player;

    public AnimationCurve EaglePath;
    float Duration = 1f;
    float time;
    public float currentTime = 10f;
    private bool isMoving = false;
    private bool startHunting = false;

    // State management
    private enum EagleState { Hunting, Lifting, Carrying, Dropping }
    private EagleState currentState = EagleState.Hunting;
    private Vector3 liftTarget;
    private Vector3 startCarryPosition;
    public HealthManager healthManager;

    override protected void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player = collision.gameObject.GetComponent<PlayerMovement>();

        if (collision.collider.CompareTag("Player") && currentState == EagleState.Hunting)
        {
            if (Player.IsSpinning())
            {
                //Die();
                // Enemy dies
                //  Destroy(gameObject);
                Player.Bounce();
            }
            else
            {
                if (healthManager.DecreaseHearts() == 0)
                {
                    Player.Die();
                    Destroy(gameObject, 1f);
                    return;
                }
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

        if (StopGame.Instance != null && StopGame.Instance.IsFrozen())
        {
            rb.linearVelocity = Vector2.zero;
            return; // skip rest of logic
        }

        if (startHunting)
        {
            currentTime -= Time.deltaTime;
            Debug.Log($"Current time : {currentTime}");
        }

        if (currentTime <= 0f)
        {
            rb.linearVelocity = Vector2.zero;
            Destroy(gameObject);
            return;
        }
        switch (currentState)
        {
            case EagleState.Hunting:
                HandleHunting();
                break;
        }
    }

    private bool isActivelyHunting = false;

    private void HandleHunting()
    {

        if (!isActivelyHunting)
        {
            if (!ShouldBeActive())
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (player != null)
            {
                isActivelyHunting = true;
            }
        }

        if (player != null)
        {


            direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * swoopSpeed;
            startHunting = true;
            HandleFlipping();
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
