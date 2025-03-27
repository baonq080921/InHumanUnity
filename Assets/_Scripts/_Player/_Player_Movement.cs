using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class _Player_Movement : MonoBehaviour
{

    private ThirdPersonActionAssets controller;
    [SerializeField] CharacterController characterController;

    private Player player;


    public Vector2 moveInput{get; private set;}
    [SerializeField] Vector2 aimInput;
    [SerializeField] float speed;


    [Header("Walking")]
    private float walkSpeed = 2f;
    private Vector3 moveDir;


    [Header("Gravity")]
    private Vector3 playerVelocity;
    [SerializeField] private float gravityScale = 9.81f;
    private bool groundedPlayer;



    [Header("Running")]
    private bool isRunning;
    private float runSpeed = 5f;



    [Header("Aim Info")]
    [SerializeField] float _aimSpeed;
    [SerializeField] private Transform _aimTransform;
    [SerializeField] private Transform _playerMainTransform;

    [Header("animation")]
    private Animator animator;

    [SerializeField] float dapTime = .1f;

    void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        speed = walkSpeed;
        ApplyInputEvents();

    }
    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorController();
    }



    void ApplyInputEvents()
    {
        controller = player.controls;
        controller.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controller.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controller.Player.Run.performed += ctx =>{
            isRunning = true;
            if(isRunning) speed = runSpeed;
        }; 
        controller.Player.Run.canceled += ctx =>{
            isRunning = false;
            if(!isRunning) speed = walkSpeed;
        }; 
    }

    void ApplyMovement(){
        moveDir = new Vector3(moveInput.x,0,moveInput.y);
        if(moveDir.magnitude > 0){
            characterController.Move(moveDir * speed * Time.deltaTime);
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


    

    void ApplyRotation(){
            Vector3 lookingDir = player.aim.getMouseHitInfo().point - _playerMainTransform.position;
            lookingDir.y = 0f;
            lookingDir.Normalize();
             Quaternion targetRotation = Quaternion.LookRotation(lookingDir);
            _playerMainTransform.rotation = Quaternion.Slerp(_playerMainTransform.rotation, targetRotation, _aimSpeed * Time.deltaTime);
            
    }


    void AnimatorController(){
        //checking direction of the player:
        float xVelocity = Vector3.Dot(moveDir.normalized,transform.right);
        float yVelocity = Vector3.Dot(moveDir.normalized, transform.forward);
        animator.SetFloat("xVelocity",xVelocity,dapTime,Time.deltaTime);
        animator.SetFloat("yVelocity",yVelocity,dapTime,Time.deltaTime);
        bool playRunAnimation =  isRunning && moveDir.magnitude > 0;
        animator.SetBool("isRunning",playRunAnimation);
    }

}
