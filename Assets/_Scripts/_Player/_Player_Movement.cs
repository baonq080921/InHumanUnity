using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class _Player_Movement : MonoBehaviour
{
    private ThirdPersonActionAssets playerActionAsset;
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
    [SerializeField] private Transform _playerMainTransform;

    [Header("animation")]
    private Animator animator;
    [SerializeField] float dapTime = 1f;
    private float

    

    void Awake(){
        playerActionAsset = new ThirdPersonActionAssets();
        playerActionAsset.Player.Fire.performed += ctx =>{Debug.Log(ctx);};
        //================Movement ======================
        playerActionAsset.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerActionAsset.Player.Move.canceled += ctx => moveInput = Vector2.zero;
 
        //===============Aim==============
        playerActionAsset.Player.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        playerActionAsset.Player.Aim.canceled += ctx => aimInput = Vector2.zero;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
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
        AnimatorController();
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
        animator.SetTrigger("isShooting");
    }

    void ApplyAim(){
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,Mathf.Infinity,_aimLayerMask)){
            Vector3 lookingDir = hit.point - _playerMainTransform.position;
            lookingDir.y = 0f;
            lookingDir.Normalize();
             Quaternion targetRotation = Quaternion.LookRotation(lookingDir);
            _playerMainTransform.rotation = Quaternion.Lerp(_playerMainTransform.rotation, targetRotation, _aimSpeed * Time.deltaTime);
            
            _aimTransform.position = new Vector3(hit.point.x,_playerMainTransform.position.y+.5f,hit.point.z);
        }
    }


    void AnimatorController(){
        float xVelocity = Vector3.Dot(moveDir.normalized,transform.right);
        float yVelocity = Vector3.Dot(moveDir.normalized, transform.forward);
        animator.SetFloat("xVelocity",xVelocity,dapTime,Time.deltaTime);
        animator.SetFloat("yVelocity",yVelocity,dapTime,Time.deltaTime);

    }

}
