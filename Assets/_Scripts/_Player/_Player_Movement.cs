using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class _Player_Movement : MonoBehaviour
{

    private ThirdPersonActionAssets playerActionAsset;
    private Rigidbody rb;
    [SerializeField] Vector2 moveInput;
    [SerializeField] Vector2 aimInput;
    [SerializeField] CharacterController characterController;

    [Header("Walking")]
    [SerializeField] private float walkSpeed = 2f;
    private Vector3 moveDir;
    [Header("Gravity")]
    private Vector3 playerVelocity;
    [SerializeField] private float gravityScale = 9.81f;
    private bool groundedPlayer;

    [Header("Aim Info")]
    [SerializeField] private LayerMask _aimLayerMask;
    [SerializeField] float _aimSpeed;
    [SerializeField] private Transform _aimTransform;


    

    void Awake(){
        playerActionAsset = new ThirdPersonActionAssets();
        playerActionAsset.Player.Fire.performed += ctx => Shoot();
        //================Movement ======================
        playerActionAsset.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerActionAsset.Player.Move.canceled += ctx => moveInput = Vector2.zero;
 
        //===============Aim==============
        playerActionAsset.Player.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        playerActionAsset.Player.Aim.canceled += ctx => aimInput = Vector2.zero;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        playerActionAsset.Enable();
    }

    void OnDisable()
    {
        playerActionAsset.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyAim();
    }


    void ApplyMovement(){
        moveDir = new Vector3(moveInput.x,0,moveInput.y);
        if(moveDir.magnitude > 0){
            characterController.Move(moveDir * walkSpeed * Time.deltaTime);
        }
    }


    void ApplyGravity(){
        groundedPlayer = characterController.isGrounded;
        if(groundedPlayer && playerVelocity.y < 0f){
            playerVelocity.y = 0f;
        }

        playerVelocity.y -= gravityScale * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
    void Shoot(){
        Debug.Log("Shoot");
    }

    void ApplyAim(){
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,Mathf.Infinity,_aimLayerMask)){
            Vector3 lookingDir = hit.point - transform.position;
            lookingDir.y = 0f;
            lookingDir.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(lookingDir);
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,_aimSpeed);

            _aimTransform.position = new Vector3(hit.point.x,transform.position.y,hit.point.z);
        }
    }

}
