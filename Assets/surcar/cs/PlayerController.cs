using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 100f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    public bool isGrounded;
    private InputManager inputManager;
    public float speed;
    //private WeaponSystem weaponSystem;

    private float xRotation = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        //weaponSystem = GetComponent<WeaponSystem>();

        // 隐藏并锁定鼠标
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    private void Update()
    {
        HandleAttack();
        HandleAction();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        Vector3 movement = new Vector3(inputManager.MoveInput.x, 0f, inputManager.MoveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + transform.TransformDirection(movement));
        speed = (new Vector3(inputManager.MoveInput.x, 0f, inputManager.MoveInput.y) * moveSpeed).magnitude;
    }

    private void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        if (inputManager.JumpPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleAttack()
    {
        if (inputManager.AttackPressed)
        {
            //weaponSystem.Attack();
        }
    }

    private void HandleAction()
    {
        if (inputManager.ActionPressed)
        {
            PlayerInteract.Instance.TakeInteract(3);
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = inputManager.MouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = inputManager.MouseInput.y * mouseSensitivity * Time.deltaTime;

        // 左右旋转角色
        transform.Rotate(Vector3.up * mouseX);

        // 上下旋转摄像机
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}