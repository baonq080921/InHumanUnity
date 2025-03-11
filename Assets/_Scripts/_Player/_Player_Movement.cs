using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class _Player_Movement : MonoBehaviour
{


    [SerializeField] Vector2 moveInput;
    [SerializeField] Vector2 aimInput;
    [SerializeField] CharacterController characterController;
    private Vector3 moveDir;
    [SerializeField] private float walkSpeed = 2f;

    private Rigidbody rb;
    private ThirdPersonActionAssets playerActionAsset;
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
        moveDir = new Vector3(moveInput.x,0,moveInput.y);

       if(moveDir.magnitude > 0){
        characterController.Move(moveDir * walkSpeed * Time.deltaTime );
       }
        
    }

    void FixedUpdate()
    {
       
    }

    void Shoot(){
        Debug.Log("Shoot");
    }

}
