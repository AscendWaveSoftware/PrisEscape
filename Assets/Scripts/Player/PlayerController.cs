using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [Space]
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerAcceleration = 3f;

    [Header("Player Jump Settings")]
    [Space]
    [SerializeField] private float playerJumpForce = 10f;
    [SerializeField] private Transform playerRoot;
    private bool isGrounded;

    [Header("Player Camera Settings")]
    [Space]
    [SerializeField] private float playerLookSensitivity = 20f;
    [SerializeField] private float cameraClamp = 89f;
    [SerializeField] private Transform cameraTransform;
    private float cameraRotationX;
    private float cameraRotationY;

    [Header("Player Input Reference")]
    [Space]
    [SerializeField] private PlayerInput playerInput;

    private Rigidbody rb;
    private InputAction playerMove;
    private InputAction playerLook;
    private InputAction playerJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        playerMove = playerInput.actions["Move"];
        playerLook = playerInput.actions["Look"];
        playerJump = playerInput.actions["Jump"];
    }

    private void Start()
    {
        playerJump.started += Jump;
    }

    private void OnDisable()
    {
        playerJump.started -= Jump;
    }

    private void Update()
    {
        GroundCheck();
    }

    private void FixedUpdate()
    {
        VelocityChange(PlayerDirection());
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    private void VelocityChange(Vector3 _playerDirection)
    {
        var temp = _playerDirection;

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(temp.x, 0f, temp.z);
        targetVelocity *= playerSpeed;

        targetVelocity = cameraTransform.TransformDirection(targetVelocity);

        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange.y = 0f;
        velocityChange = Vector3.ClampMagnitude(velocityChange, playerAcceleration);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private Vector3 PlayerDirection()
    {
        var temp = playerMove.ReadValue<Vector2>();
        return new Vector3(temp.x, 0f, temp.y);
    }

    private Vector2 GetRotation()
    {
        return playerLook.ReadValue<Vector2>();
    }

    private void UpdateCamera()
    {
        var rotation = GetRotation();

        cameraRotationY += -rotation.y * playerLookSensitivity * Time.deltaTime;
        cameraRotationY = Mathf.Clamp(cameraRotationY, -cameraClamp, cameraClamp);

        cameraRotationX += rotation.x * playerLookSensitivity * Time.deltaTime;

        cameraTransform.eulerAngles = new Vector3(cameraRotationY, cameraRotationX, cameraTransform.eulerAngles.z);
    }

    private void Jump(InputAction.CallbackContext _context)
    {
        if (_context.started && isGrounded)
            rb.AddForce(new Vector3(0, playerJumpForce, 0), ForceMode.Impulse);
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(playerRoot.position, Vector3.down, 0.3f) == true)
            isGrounded = true;
        else
            isGrounded = false;
    }
}
