using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Player player;
    private ThirdPersonActionAssets controls;
    private Vector2 aimInput;

    [Header("Aim Info")]
    [SerializeField] private LayerMask _aimLayerMask;
    [SerializeField] Transform _aimTransform;
    [SerializeField] private Transform _playerMainTransform;
    [SerializeField] public float _aimSpeed = 10f;


    void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvent();
    }

    void Update()
    {
        _aimTransform.position = new Vector3(player.aim.getMousePosition().x,_playerMainTransform.position.y+.5f,player.aim.getMousePosition().z);  

    }


    private void AssignInputEvent()
    {
        controls = player.controls;
        controls.Player.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => aimInput = Vector2.zero;
    }

    public Vector3 getMousePosition(){
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity,_aimLayerMask)){
            return hitInfo.point;
        }
        return Vector3.zero;
    }
}
