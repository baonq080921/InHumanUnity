using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class _Player_Action : MonoBehaviour
{
    // Player Input System:
    private ThirdPersonActionAssets playerActionAsset;
    [SerializeField] CharacterController characterController;
    private InputAction _move;
    private InputAction _fire;
    private InputAction _firePad;
    private InputAction _rotate;

    // Data fields:
    private Vector2 moveDir;
    private Vector2 rotateDir;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float speed = 10f;
    [SerializeField] LayerMask whatIsLayer;
    [SerializeField] Transform _aimTransform;
    private bool isUsingGamepad = false;

    void Awake()
    {
        playerActionAsset = new ThirdPersonActionAssets();
        _move = playerActionAsset.Player.Move;
        _fire = playerActionAsset.Player.Fire;
        _rotate = playerActionAsset.Player.Rotation;
        _firePad = playerActionAsset.Player.FirePad;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        _move.Enable(); // Enable action 
        _rotate.Enable();
        _fire.Enable();
        _firePad.Enable();
        _rotate.performed += OnRotate; // Subcripe action 
        _rotate.canceled += OnRotateStop;
        // _fire.performed += OnFire;
        // _firePad.performed += OnFire;

        // Phát hiện nếu sử dụng Gamepad
        InputSystem.onAnyButtonPress.CallOnce(ctrl => {
            isUsingGamepad = ctrl.device is Gamepad;
            Debug.Log($"Input detected from: {ctrl.device.name}, Using Gamepad: {isUsingGamepad}");
        });
    }

    void OnDisable()
    {
        _move.Disable();
        _rotate.Disable();
        _fire.Disable();
    }

    void Update()
    {
        if (!isUsingGamepad)
        {
            UpdateAim(); // CHi update khi khong su dunng gamepad
        }

        CheckInput();
    }

    private void CheckInput()
    {
        moveDir = _move.ReadValue<Vector2>();
        if (moveDir.y < 0) moveDir.x = -moveDir.x;
    }

    void FixedUpdate()
    {
        ApplyMovement();
        BodyRotation();

        if (isUsingGamepad)
        {
            TowerRotationGamepad();
        }
        else
        {
            TowerRotationMouse();
        }
    }

    // --- Quay bằng Gamepad ---
    void TowerRotationGamepad()
    {
        if (rotateDir.magnitude > 0.1f) 
        {
            float targetAngle = Mathf.Atan2(rotateDir.x, rotateDir.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            this.transform.rotation = Quaternion.RotateTowards(
                this.transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime * 10f
            );
        }
    }

    // --- Quay bằng Chuột ---
    private void TowerRotationMouse()
    {
        Vector3 direction = _aimTransform.position - this.transform.position;
        direction.y = 0;
        Quaternion targetDirection = Quaternion.LookRotation(direction);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetDirection, rotateSpeed);
    }

    private void BodyRotation()
    {
        transform.Rotate(0, moveDir.x * speed, 0);
    }

    private void ApplyMovement()
    {
        Vector3 movement = transform.forward * speed * moveDir.y;
        characterController.Move(movement);
    }

    // --- Xử lý bắn ---
    // private void OnFire(InputAction.CallbackContext ctx)
    // {
    //     Fire();
    // }

    // private void Fire()
    // {
    //     Debug.Log("Fire");

    //     GameObject bullet = Instantiate(bulletPrefab, _gunPointTransform.position, _gunPointTransform.rotation);
    //     bullet.GetComponent<Rigidbody>().linearVelocity = _gunPointTransform.forward * bulletSpeed;

    //     Destroy(bullet, 0.5f);
    // }

    // --- Xử lý Input từ Gamepad ---
    void OnRotate(InputAction.CallbackContext context)
    {
        rotateDir = context.ReadValue<Vector2>();

        if (rotateDir.magnitude > 0.1f)
        {
            isUsingGamepad = true;
        }
    }

    void OnRotateStop(InputAction.CallbackContext context)
    {
        rotateDir = Vector2.zero; 
    }

    // --- Xử lý Aim bằng Chuột ---
    void UpdateAim()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, whatIsLayer))
        {
            float fixedY = _aimTransform.position.y;
            _aimTransform.position = new Vector3(hit.point.x, fixedY, hit.point.z);
        }
    }
}
