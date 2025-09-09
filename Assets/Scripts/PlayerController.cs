using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    CharacterController playerController;

    [SerializeField] GameObject playerCamera;

    [SerializeField] Rigidbody playerRigidBody;

    [SerializeField] float playerSpeed = 1.0f;
    [SerializeField] float playerJumpForce = 1.0f;
    //[SerializeField] float playerLookSensitivityX = 1.0f;
    //[SerializeField] float playerLookSensitivityY = 1.0f;
    [SerializeField] float time = 0.0f;


    Vector2 movementInput;

    InputAction playerMovement;
    InputAction playerJump;
    //InputAction playerShoot;
    //InputAction playerSprint;
    InputAction playerLook;

    //public event Action OnStatsChanged;

    public void OnEnable()
    {

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        playerMovement = InputSystem.actions.FindAction("Move");
        playerJump = InputSystem.actions.FindAction("Jump");
        //playerShoot = InputSystem.actions.FindAction("Attack");
        //playerSprint = InputSystem.actions.FindAction("Sprint");
        playerLook = InputSystem.actions.FindAction("Look");

        //isGameOver = false;
        //isDead = false;
        //isPaused = false;
        //isMoving = false;
    }

    // Update is called once per frame
    public void Update()
    {
        UpdateMovement();
        //StaminaTicker();
        //GroundCheck();
        //PlayAreaCheck();

        time += Time.deltaTime;
    }

    public void OnMove()
    {
        //Debug.Log("I AM MOVING");
        Vector2 moveValue = playerMovement.ReadValue<Vector2>() * playerSpeed * Time.deltaTime;
        transform.Translate(moveValue.x, 0, moveValue.y);

        /*
         * while (moveValue.y == 0)
        {
            moveValue.y = moveSpeedModifier * Time.deltaTime;
            transform.Translate(0, 0, moveValue.y);
        }
        */
    }

    public void UpdateMovement()
    {
        OnMove();
    }

    void OnJump()
    {
        //if (isGrounded)
        //{
            playerRigidBody.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
        //}
    }

    /*
    public void DamagePlayer()
    {
        currentStamina -= 5;
    }
    */

    /*
    public void StaminaTicker()
    {
        if (isDead) return;
        if (time >= tickerTimer)
        {
            time = time - tickerTimer;
            currentStamina -= 1;
        }
        if (currentStamina <= 0)
        {
            isDead = true;
        }
    }
    */

    /*
     * public void Died()
    {
        if (isDead)
        {
            isGameOver = true;
        }
    }
    */

    /*
    public void GroundCheck()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    */

    /*
    public void PlayAreaCheck()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, onLevelCheckDistance))
        {
            isOnLevel = true;
        }
        else
        {
            isOnLevel = false;
        }
        if (!isOnLevel)
        {
            onLevelElapsedTime += Time.deltaTime;
            if (onLevelElapsedTime >= onLevelKillTimer)
            {
                isDead = true;
            }
        }
    }
    */
}

