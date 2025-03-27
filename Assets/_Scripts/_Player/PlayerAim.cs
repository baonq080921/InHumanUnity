using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Player player;
    private ThirdPersonActionAssets controls;
    private Vector2 aimInput;

    [Range(0f,5f)]
    [SerializeField] private float minCameraDistance = 1.5f;

    [Range(0f,5f)]
    [SerializeField] private float maxCameraDistance = 4f;

    [Range(3f,5f)]   
    [SerializeField] private float _cameraSensetivity = 5f;

    [Space]
    private RaycastHit lastKnowMouseHitInfo;
    [Header("Camera Control")]
    [SerializeField] private LayerMask _aimLayerMask;
    [SerializeField] Transform camearaTarget;
    [SerializeField] private Transform _playerMainTransform;
    [SerializeField] public float _aimSpeed = 10f;

    [Header("Aim Info")]
    [SerializeField] Transform aim;
    [SerializeField] private bool isAimingPrecisely;




    void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvent();
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P)){
            isAimingPrecisely  = !isAimingPrecisely;
        }
        UpdateCameraPosition();
        UpdateAimPositon();
    }

    private void UpdateCameraPosition()
    {
        camearaTarget.position = Vector3.Lerp(camearaTarget.position, DesireCameraPosition(), _cameraSensetivity * Time.deltaTime);
    }

    private void UpdateAimPositon()
    {
        aim.position = getMouseHitInfo().point;
        if (!isAimingPrecisely)
        {
            aim.position = new Vector3(aim.position.x, _playerMainTransform.position.y + 1f, aim.position.z);
        }
        Debug.Log("AIm position" + aim.position);

    }

    public bool CanAimPrecisely(){
        if(isAimingPrecisely){
            return true;
        }
        return false;
    }


    private void AssignInputEvent()
    {
        controls = player.controls;
        controls.Player.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => aimInput = Vector2.zero;
    }

    public RaycastHit getMouseHitInfo(){
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity,_aimLayerMask)){
            lastKnowMouseHitInfo = hitInfo;
            return hitInfo;
        }
        return lastKnowMouseHitInfo;
    }


    private Vector3 DesireCameraPosition(){
        // change the maxdistance based on the character is moving forward or backward
        float actualMaxCameraDistance = player.movement.moveInput.y  < .5f ? minCameraDistance : maxCameraDistance ;
        Vector3 desireAimPosition = getMouseHitInfo().point;
        Vector3 aimDirection = (desireAimPosition - _playerMainTransform.transform.position).normalized;
        float distanceToDesirePosition = Vector3.Distance(_playerMainTransform.transform.position, desireAimPosition);
        float clampDistance = Mathf.Clamp(distanceToDesirePosition, minCameraDistance, actualMaxCameraDistance);
        desireAimPosition = _playerMainTransform.transform.position + aimDirection * clampDistance; 
        desireAimPosition.y = _playerMainTransform.transform.position.y + 1;
        Debug.Log(actualMaxCameraDistance);

        return desireAimPosition;
    }

}
