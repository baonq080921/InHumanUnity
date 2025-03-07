using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class _Player_Action : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    // Player Input New System:
    private ThirdPersonActionAssets playerActionAsset;
    private InputAction  _move;
    private InputAction _fire;

    [Header("Tank Tower Stuff")]
    [SerializeField] Transform _tankTowerTransform;
    [SerializeField]  Transform _aimTransform;


    [Header("Fire, Shooting Stuff")]
    [SerializeField] Transform _gunPointTransform;
    [SerializeField] float bulletSpeed;

    [SerializeField] GameObject bulletPrefab;

    // Data fields :
    private Vector2 moveDir;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float speed = 10f;
    [SerializeField] LayerMask whatIsLayer;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerActionAsset = new ThirdPersonActionAssets();
        _move  = playerActionAsset.Player.Move;
        _fire = playerActionAsset.Player.Fire;


    }

    void OnEnable()
    {
        // _rotation.performed += OnRotation;
        _move.Enable();
        _fire.performed += OnFire; 
        _fire.Enable();
        
    }

    void OnDisable()
    {
        _move.Disable();   
        _fire.Disable();
    }

    void Update()
    {

        UpdateAim();
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
        TowerRotation();

    }

    private void TowerRotation()
    {
        Vector3 direction = _aimTransform.position - _tankTowerTransform.position;
        direction.y = 0;
        Quaternion targetDirecion = Quaternion.LookRotation(direction);
        _tankTowerTransform.rotation = Quaternion.RotateTowards(_tankTowerTransform.rotation, targetDirecion, rotateSpeed);
    }

    private void BodyRotation()
    {
        this.transform.Rotate(0, moveDir.x * speed, 0);
    }

    private void ApplyMovement()
    {
        Vector3 movement = this.transform.forward * speed * moveDir.y;
        rb.linearVelocity = movement;
    }


private void OnFire(InputAction.CallbackContext ctx)
    {
        Fire();
    }
    private void Fire(){
        Debug.Log("Fire ");

        GameObject bullet = Instantiate(bulletPrefab,_gunPointTransform.position,_gunPointTransform.rotation);

        bullet.GetComponent<Rigidbody>().linearVelocity = _gunPointTransform.forward * bulletSpeed;

        Destroy(bullet,.5f);

    }
    //      // GamePad
    //    void OnRotation(InputAction.CallbackContext ctx)
    // {
    //     rotateDir = _rotation.ReadValue<Vector2>();

    //     // Nếu gamepad đang được sử dụng
    //         isUsingGamepad = rotateDir.magnitude > 0.1f;

    //     if (isUsingGamepad) 
    //     {
    //         float targetAngle = Mathf.Atan2(rotateDir.x, rotateDir.y) * Mathf.Rad2Deg;
    //         Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

    //         _tankTowerTransform.rotation = Quaternion.RotateTowards(
    //             _tankTowerTransform.rotation,
    //             targetRotation,
    //             rotateAngle * Time.deltaTime * 10f
    //         );
    //     }
    // }



    // Mouse Rotation:
    void UpdateAim(){
        Vector2 mousePostion = Mouse.current.position.ReadValue();
        Ray ray =  Camera.main.ScreenPointToRay(mousePostion);

        RaycastHit hit;

        if(Physics.Raycast(ray,out hit,Mathf.Infinity,whatIsLayer)){
            float fixedY = _aimTransform.position.y;
            _aimTransform.position = new Vector3(hit.point.x,fixedY,hit.point.z);
        }
    }




}
