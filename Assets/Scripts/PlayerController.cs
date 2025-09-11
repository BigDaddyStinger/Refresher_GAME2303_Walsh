using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
[SerializeField] GameObject playerCamera;
[SerializeField] Rigidbody playerRigidBody;
[SerializeField] Animator animator;
[SerializeField] ScriptableObject playerStats;

[SerializeField] float walkSpeed = 3.5f;
[SerializeField] float runMultiplier = 2f;
[SerializeField] float jumpForce = 5f;

[SerializeField] float lookSensitivity = 150f;  // deg/sec
[SerializeField] float minPitch = -80f;
[SerializeField] float maxPitch = 80f;

[SerializeField] Transform groundCheck;
[SerializeField] float groundRayDistance = 0.25f;
[SerializeField] LayerMask groundMask = ~0; // everything by default
[SerializeField] float groundRayStartOffset = 0.05f;

InputAction actMove;
InputAction actJump;
InputAction actSprint;
InputAction actLook;

Vector2 moveInput;
Vector2 lookInput;
bool sprintHeld = false;

float yaw;
float pitch;

bool isGrounded = false;

void Start()
{
    actMove = InputSystem.actions.FindAction("Move");
    actJump = InputSystem.actions.FindAction("Jump");
    actSprint = InputSystem.actions.FindAction("Sprint");
    actLook = InputSystem.actions.FindAction("Look");

    if (actSprint != null)
    {
        actSprint.performed += _ => sprintHeld = true;
        actSprint.canceled += _ => sprintHeld = false;
    }

    yaw = transform.eulerAngles.y;
    pitch = playerCamera != null ? playerCamera.transform.localEulerAngles.x : 0f;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    animator.SetBool("isMoving", false);
    animator.SetBool("isSprinting", false);
    animator.SetBool("isGrounded", true);
    animator.SetFloat("walkSpeed", 0f);
    animator.SetFloat("verticalSpeed", 0f);
}

void OnDisable()
{
    if (actSprint != null)
    {
        actSprint.performed -= _ => sprintHeld = true;
        actSprint.canceled -= _ => sprintHeld = false;
    }
}

void Update()
{
    UpdateLook();

    GroundCheck();   
        
    UpdateMoveAndAnim();

    SprintSafetyUnstick();
}

void UpdateLook()
{
    if (actLook == null || playerCamera == null) return;

    lookInput = actLook.ReadValue<Vector2>();
    float dt = Time.deltaTime;

    yaw += lookInput.x * lookSensitivity * dt;
    pitch -= lookInput.y * lookSensitivity * dt;
    pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

    transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
}

void UpdateMoveAndAnim()
{
    if (actMove == null) return;

    moveInput = actMove.ReadValue<Vector2>(); // raw -1..1
    Vector3 fwd = transform.forward; fwd.y = 0f; fwd.Normalize();
    Vector3 right = transform.right; right.y = 0f; right.Normalize();

    Vector3 moveDir = (fwd * moveInput.y) + (right * moveInput.x);
    bool isMovingInput = moveDir.sqrMagnitude > 0.0001f;

    float speed = walkSpeed * (sprintHeld ? runMultiplier : 1f);

    float airControl = isGrounded ? 1f : 0.6f;
    if (isMovingInput)
    {
        transform.Translate(moveDir.normalized * speed * airControl * Time.deltaTime, Space.World);
        transform.forward = moveDir.normalized; // face move direction
    }

    animator.SetBool("isMoving", isMovingInput);
    animator.SetBool("isSprinting", isMovingInput && sprintHeld);
    animator.SetFloat("walkSpeed", Mathf.Clamp01(moveInput.magnitude));
    animator.SetFloat("verticalSpeed", playerRigidBody != null ? playerRigidBody.linearVelocity.y : 0f);
}

public void OnJump()
{
    if (playerRigidBody == null) return;

    if (!isGrounded) return;

    playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    animator.SetTrigger("isJumping");
}

void GroundCheck()
{
    Vector3 origin = (groundCheck != null ? groundCheck.position : transform.position)
                        + Vector3.up * groundRayStartOffset;

    bool hit = Physics.Raycast(origin, Vector3.down, out RaycastHit info, groundRayDistance + groundRayStartOffset, groundMask, QueryTriggerInteraction.Ignore);

    isGrounded = hit;

    animator.SetBool("isGrounded", isGrounded);
}

void SprintSafetyUnstick()
{
    if (sprintHeld && actSprint != null && !actSprint.IsPressed())
        sprintHeld = false;
}

void OnDrawGizmosSelected()
{
    Vector3 origin = (groundCheck != null ? groundCheck.position : transform.position)
                        + Vector3.up * groundRayStartOffset;
    Gizmos.color = Color.green;
    Gizmos.DrawLine(origin, origin + Vector3.down * (groundRayDistance + groundRayStartOffset));
}
}
