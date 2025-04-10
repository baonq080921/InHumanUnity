using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    private Player player;
    private ThirdPersonActionAssets controls;
    private Vector2 mouseInput;

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

    [Header("Aim Info")]
    [SerializeField] Transform aim;
    [SerializeField] public float _aimSpeed = 10f;
    [SerializeField] private bool isAimingPrecisely;
    [SerializeField] private bool isLockingToTarget;
    
    [Header("Aim Laser")]
    [SerializeField] LineRenderer aimLaser;



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
        if(Input.GetKeyDown(KeyCode.L)){
            isLockingToTarget = !isLockingToTarget;
        }
        UpdateCameraPosition();
        UpdateAimPositon();
        UpdateLaserVisual();
    }

   public Transform Target(){
        Transform target = null;
        if(getMouseHitInfo().transform.GetComponent<Target>() != null){
            target = getMouseHitInfo().transform;
            // Debug.Log("target positon"+target.position);
        }
        return target;
    }



    private void UpdateLaserVisual(){
        float laserTipLength = .5f;
        Transform gunPoint = player.weapon.GunPoint();
        Vector3 aimLaserDirection = player.weapon.bulletDirection();
        float laserDistance = 4f;
        Vector3 endPoint = gunPoint.position + aimLaserDirection * laserDistance;

        // CHeck if the laser hit the obstacle then update the endPoint to make the laser stop at the obstacle
        if(Physics.Raycast(gunPoint.position,aimLaserDirection,out RaycastHit hitInfo,laserDistance,_aimLayerMask)){
            endPoint = hitInfo.point;
            laserTipLength = 0f;
        }
        aimLaser.SetPosition(0,gunPoint.position);
        aimLaser.SetPosition(1,endPoint);
        aimLaser.SetPosition(2,endPoint + aimLaserDirection * laserTipLength);

    }

    

    private void UpdateAimPositon()
    {

        //Locking to the target if the target is not null
        Transform target = Target();
        if(target != null && isLockingToTarget){
            if(target.GetComponent<Renderer>() != null) aim.position = target.GetComponent<Renderer>().bounds.center;
            else aim.position = target.position;
            return;
        }

        

        aim.position = getMouseHitInfo().point;
        if (!isAimingPrecisely)
        {
            aim.position = new Vector3(aim.position.x, _playerMainTransform.position.y + 1f, aim.position.z);
        }
        // Debug.Log("AIm position" + aim.position);

    }

    public Transform Aim() => aim;

    public bool CanAimPrecisely(){
        if(isAimingPrecisely){
            return true;
        }
        return false;
    }


    private void AssignInputEvent()
    {
        controls = player.controls;
        controls.Player.Aim.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => mouseInput = Vector2.zero;
    }

    public RaycastHit getMouseHitInfo(){
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity,_aimLayerMask)){
            lastKnowMouseHitInfo = hitInfo;
            return hitInfo;
        }
        return lastKnowMouseHitInfo;
    }



#region Camera Region
    private Vector3 DesireCameraPosition(){
        // change the maxdistance based on the character is moving forward or backward
        float actualMaxCameraDistance = player.movement.moveInput.y  < .5f ? minCameraDistance : maxCameraDistance ;
        Vector3 desireAimPosition = getMouseHitInfo().point;
        Vector3 aimDirection = (desireAimPosition - _playerMainTransform.transform.position).normalized;
        float distanceToDesirePosition = Vector3.Distance(_playerMainTransform.transform.position, desireAimPosition);
        float clampDistance = Mathf.Clamp(distanceToDesirePosition, minCameraDistance, actualMaxCameraDistance);
        desireAimPosition = _playerMainTransform.transform.position + aimDirection * clampDistance; 
        desireAimPosition.y = _playerMainTransform.transform.position.y + 1;
        // Debug.Log(actualMaxCameraDistance);

        return desireAimPosition;
    }


    private void UpdateCameraPosition()
    {
        camearaTarget.position = Vector3.Lerp(camearaTarget.position, DesireCameraPosition(), _cameraSensetivity * Time.deltaTime);
    }
    
#endregion

}
