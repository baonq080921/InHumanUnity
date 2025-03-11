using UnityEngine;
using UnityEngine.UIElements;

public class _Player_Movement : MonoBehaviour
{

    [SerializeField] Vector2 moveDir;
    [SerializeField] Vector2 aimDir;

    [SerializeField] Rigidbody rb;
    private ThirdPersonActionAssets playerActionAsset;
    void Awake(){

        rb = GetComponent<Rigidbody>();
        playerActionAsset = new ThirdPersonActionAssets();
        playerActionAsset.Player.Fire.performed += ctx => Shoot();
        //================Movement ======================
        playerActionAsset.Player.Move.performed += ctx => moveDir = ctx.ReadValue<Vector2>();
        playerActionAsset.Player.Move.canceled += ctx => moveDir = Vector2.zero;
 
        //===============Aim==============
        playerActionAsset.Player.Aim.performed += ctx => aimDir = ctx.ReadValue<Vector2>();
        playerActionAsset.Player.Aim.canceled += ctx => aimDir = Vector2.zero;
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
        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDir.x,0, moveDir.y);
    }

    void Shoot(){
        Debug.Log("Shoot");
    }

}
