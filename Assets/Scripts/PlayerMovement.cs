using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private List<GameObject> power = new List<GameObject>();

    public GameObject CurrentPowerUp;
    CurrentPowerUp script;

    public GameObject MainMenu;
   

    public GameObject seedLogic;
    SeedLogic seedScript;
    private enum DeathType { None, Normal, Final }
    private DeathType currentDeath = DeathType.None;
    private bool isDead = false;

    [SerializeField]
    private GameObject GameOverScreen;
    public GameObject WinScreen;

    private float RunningSpeed = 8f;
    private float JumpVelocity = 14f;
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
    public bool IsFrozen = false;
    // public Transform handPoint;      
    public GameObject rockPrefab;
    public float throwForce = 10f;
    public Transform handPointt;
    public bool doubleJumpUnlocked = false;
    public bool HasKey;
    public bool ShouldFly = false;

    public Vector3 PlayerPos;
    public Transform PlayerSpawnPoint;
    public Transform AppleAttachement;

    public bool LiquidPicked;

    public Rigidbody2D rb;

    [SerializeField] public bool hasSpinPower = false;
    [SerializeField] private float spinDelay = 0.8f;    // small delay after jump before spin can start
    [SerializeField] private float spinBounceVelocity = 10f;
    [SerializeField] private float spinGravityScale = 1.2f;
    private float defaultGravityScale;

    private bool isSpinning = false;
    private float jumpStartTime = -999f;

    public Transform DropPoint;

    public bool IsSpinning() => isSpinning;
    public bool IsFalling() => rb.linearVelocity.y < -0.05f;

    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;
    public HealthManager healthManager;

    public float originalGravityScale = 1f; // Store original gravity

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;
        GameOverScreen.SetActive(false);
        GameOverScreen.GetComponentInChildren<Button>().onClick.AddListener(ReloadCurrentScene);
        WinScreen.GetComponentInChildren<Button>().onClick.AddListener(ReloadCurrentScene);
        // MainMenu.GetComponentInChildren<Button>().onClick.AddListener(StartGame);
        SetTimeScale(0);
       
    }
    private void OnDestroy()
    {
        
        SetTimeScale(1f);
    }

    private void Start()
    {
        originalGravityScale = rb.gravityScale; // Store original gravity scale
         script = CurrentPowerUp.GetComponent<CurrentPowerUp>();
       seedScript = seedLogic.GetComponent<SeedLogic>();
        tree1.SetActive(true);
        tree2.SetActive(false);
        tree3.SetActive(false);
        
    }

    void Update()
    {

        if (isDead) return;
        if (IsFrozen)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isMoving", false);
            return;
        }

        AnimationController();
        isGrounded = IsGrounded();

        if (ShouldFly)
        {
            FLyWithEagle();
        }

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

        //Debug.Log($"can throw : {CanThrowRock}");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
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
            { 
                DoubleJump();
                }

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
        SoundManager.Instance.PlayJump();
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

    public void FLyWithEagle()
    {
        transform.position = AppleAttachement.transform.position;
        rb.gravityScale = 0;
    }

    private void SpinJump()
    {
        anim.SetTrigger("SpinJump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpVelocity * 1.3f);
        isGrounded = false;
    }

    private void StartSpin()
    {
        isSpinning = true;
        anim.SetBool("isSpinning", true);
        rb.gravityScale = spinGravityScale;
        SoundManager.Instance.PlaySpinJump();
    }

    private void EndSpin()
    {
        isSpinning = false;
        //hasSpinPower = false;
        anim.SetBool("isSpinning", false);
        rb.gravityScale = defaultGravityScale;
        SoundManager.Instance.StopSFX();
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

        SoundManager.Instance.PlayThrow();
        Destroy(rock, 1.5f);
    }
    public void canThrowRock()
    {
        CanThrowRock = true;
        handlePowerUps("Throw");
    }
    public void Die()
    {
        /*        if (isDead) return;

                isDead = true;*/
        // currentDeath = DeathType.Normal;

        // StopGame.Instance.FreezeAllEnemies();
        HealthManager.instance.DecreaseHearts();
            if(HealthManager.instance.HeartCount == 0)
            {
                anim.SetBool("isDead", true);
                
                StartCoroutine(stopAnimation());

        }
            // anim.SetBool("isDead", false);
            
        

        //StartCoroutine(HandleDeath());
    }

    IEnumerator stopAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        FinalDeath();
       // StopGame.Instance.UnfreezeAllEnemies();
    }

    public void FinalDeath()
    {

        if (isDead) return;
        GameOverScreen.SetActive(true);
        SoundManager.Instance.PlayGameOver();
        SoundManager.Instance.StopMainTheme();

        rb.sharedMaterial = null;
        isDead = true;
        //currentDeath = DeathType.Final;
        Debug.Log("Final death");
        SoundManager.Instance.PlayPlayerHit();
        StartCoroutine(stopGame());
        
        // StartCoroutine(HandleDeath());
    }

    IEnumerator stopGame()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }

    private System.Collections.IEnumerator HandleDeath()
    {
        // Get the animation length
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float waitTime = 1f;

        yield return new WaitForSeconds(waitTime);

        if (currentDeath == DeathType.Normal)
        {
            Respawn();
            SoundManager.Instance.PlayPlayerHit();
        }
        else if (currentDeath == DeathType.Final)
            GameOver();
    }
    private void Respawn()
    {
        transform.position = PlayerSpawnPoint.position;
        SoundManager.Instance.PlayMainTheme();
        // Reset state
        anim.SetBool("isDead", false);
        isDead = false;
        currentDeath = DeathType.None;
        handlePowerUps("Reset");
        ResetPowerUps();
        EagleSpawner spawner = Object.FindFirstObjectByType<EagleSpawner>();
        if (spawner != null)
        {
            spawner.RespawnEagles();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
      
    }


    public void handlePowerUps(string powerUpName)
    {
        script.HandleCurrentPowerUp_UI(powerUpName);
        switch (powerUpName)
        {
            case "DoubleJump":
                {
                    CanThrowRock = false;
                    hasSpinPower = false;
                    CanPickRock = false;
                    HasKey = false;
                    
                    
                    break;
                }
            case "Throw":
                {
                    canDoubleJump = false;
                    hasSpinPower = false;
                    doubleJumpUnlocked = false;
                    HasKey = false;
                    break;
                }
            case "SpinJump":
                {
                    CanThrowRock = false;
                    canDoubleJump = false;
                    CanPickRock = false;
                    doubleJumpUnlocked = false;
                    HasKey = false;
                    break;
                }
            case "Key":
                {
                    CanThrowRock = false;
                    canDoubleJump = false;
                    hasSpinPower = false;
                    CanPickRock = false;
                    doubleJumpUnlocked = false;

                    HasKey = true;
                    break;
                }
            case "Reset":
                {
                    CanThrowRock = false;
                    canDoubleJump = false;
                    hasSpinPower = false;
                    CanPickRock = false;
                    doubleJumpUnlocked = false;
                    HasKey = false;
                    break;
                }
        }
    }


    public void SeedCollected()
    {
        transform.position = new Vector3(PlayerSpawnPoint.position.x, PlayerSpawnPoint.position.y);
        handlePowerUps("Reset");
        ResetPowerUps();
        Debug.Log("inside seedCollected() in player");
    }

    public void ReloadCurrentScene()
    {
        
        SoundManager.Instance.PlayMainTheme();
        SoundManager.Instance.StopGameOver();
        SeedLogic.ResetSeedCount();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }

    public void StartGame()
    {
        MainMenu.SetActive(false);
    }

    public void ResetPowerUps()
    {
        foreach (GameObject go in power)
        {
            go.SetActive(true);
            Debug.Log($"setactive {go}");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PowerUpLogic>(out _) && !collision.TryGetComponent<Key>(out _))
        {
            power.Add(collision.gameObject);
        }
        if (collision.CompareTag("HealthHeart"))
        {
            healthManager.RestoreHeats();
            Destroy(collision.gameObject);
        }
     

    }
}
