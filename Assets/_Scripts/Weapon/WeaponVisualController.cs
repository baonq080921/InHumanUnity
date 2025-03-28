using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class WeaponVisualController : MonoBehaviour
{

    private ThirdPersonActionAssets controls;

    [Header("InputSystem")]
    private Player player;
    private Rig rig;

    private Animator animator;
    
    #region GunTransfrom region
    [SerializeField] private WeaponModel[] weaponModels;
    
    #endregion
    


    [Header("Left Hand")]
    [SerializeField]Transform leftHandIK_Target;
    [SerializeField] TwoBoneIKConstraint leftHand_IK;
    private bool leftHand_IKShouldIncrease;
    private bool busyGrabingWeapon;
    [SerializeField] float leftHandIK_IncreaseStep  = 0.5f;




    [Header("Reload")]
    [SerializeField] float rigIncreaseRate = .1f; 
    private bool rigShouldIncrease;
    [SerializeField] float rigIncreaseStep = 0.5f;    

   

    void Start()
    {
        player = GetComponent<Player>();
        rig = GetComponentInChildren<Rig>();        
        animator = GetComponentInChildren<Animator>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
    }


    public WeaponModel GetCurrentWeaponModel()
    {
        WeaponModel weaponModel = null;
        WeaponType weaponType =  player.weapon.CurrentWeapon().weaponType;
        foreach (var item in weaponModels)
        {
            if(item.weaponType == weaponType)
            {
                weaponModel = item;
            }
        }
        return weaponModel;
    }
    public  void PlayReloadAnimation()
    {
        if(busyGrabingWeapon) return;

        animator.SetTrigger("isReloading");
        ReduceRig();
    }

    void Update()
    {
        // CheckWeaponSwitch();
        UpdateRig();
        UpdateLeftHandIKWeight();
        Debug.Log("Helloo");
        
    }

    void UpdateRig(){
        if(rigShouldIncrease){
            rig.weight = Mathf.MoveTowards(rig.weight,1f,rigIncreaseStep * Time.deltaTime) ;
        }
        if (rig.weight >= 1f)
        {
            rigShouldIncrease = false;
        }
    }
    public void ReturnRigWeightOne() => rigShouldIncrease = true;

    public void ReturnIkWeightOne() => leftHand_IKShouldIncrease = true;

    
    public void ReturnRigWeightToOne()=> rigShouldIncrease = true;

    public void ReturnLeftHand_IkWeightToOne() => leftHand_IKShouldIncrease = true;

    //Checking Weapong Switching toggle between keycap {1,2,3,4}
//     private void CheckWeaponSwitch(){
//     if (Input.anyKeyDown)
//     {
//         Dictionary<KeyCode,(int, weapongGrab)> keyInputMap = new Dictionary<KeyCode, (int,weapongGrab)>{
//             {KeyCode.Alpha1,(1,weapongGrab.sideGrab)},
//             {KeyCode.Alpha2,(1,weapongGrab.backGrab)},
//             {KeyCode.Alpha3,(1,weapongGrab.sideGrab)},
//             {KeyCode.Alpha4,(2,weapongGrab.backGrab)},
//             {KeyCode.Alpha5,(3,weapongGrab.backGrab)},
//         };

//         foreach(var entry in keyInputMap){
//             if(Input.GetKeyDown(entry.Key)){
//                 SwitchOnWeaponModel();
//                 SwithAnimatorLayer(entry.Value.Item1);
//                 SwitchingGun(entry.Value.Item2);
//                 break;
//             }
//         }
//     }
// }



    // Checking Gun Enable and Disable Gun Visual
    public void SwitchOnWeaponModel(){
        int aimIndex = (int)player.weapon.CurrentWeapon().weaponType;
        SwitchOffWeaponModel();
        SwithAnimatorLayer(aimIndex);
        GetCurrentWeaponModel().gameObject.SetActive(true);

       AttachLeftHand();
    }
    void SwitchOffWeaponModel(){
        foreach(WeaponModel item in weaponModels){
            item.gameObject.SetActive(false);
        }
    }


    // Hand Transfromation for each weapon correspoding
    void AttachLeftHand(){
        Transform leftHandGunTransform = GetCurrentWeaponModel().GetComponentInChildren<LeftHandTargetTransform>().transform;
        leftHandIK_Target.localPosition = leftHandGunTransform.localPosition;
        leftHandIK_Target.localRotation = leftHandGunTransform.localRotation;
    }


    // Switch the layer of the Animator with the index from 1 to n;
    void SwithAnimatorLayer(int layerIndex){
        for(int i = 1; i < animator.layerCount; i++){
            animator.SetLayerWeight(i,0);
        }
        animator.SetLayerWeight(layerIndex,1);
    }

    void SwitchingGun(weapongGrab grab)
    {
        animator.SetFloat("weaponTypeGrab", (float)grab);
        animator.SetTrigger("WeaponGrab");
        ReduceLeftHandIK_Weight();
        ReduceRig();
        SetBusyGrabingWeapon(true);

    }

    private void ReduceLeftHandIK_Weight()
    {
        leftHand_IK.weight = 0.15f;
    }

    private void ReduceRig()
    {
        rig.weight = 0f;
    }

    void UpdateLeftHandIKWeight()
    {
        if (leftHand_IKShouldIncrease)
        {
            leftHand_IK.weight = Mathf.MoveTowards(leftHand_IK.weight, 1f, leftHandIK_IncreaseStep * Time.deltaTime);
        }
        if (leftHand_IK.weight >= 1f)
        {
            leftHand_IKShouldIncrease = false;
        }

    }

    public void SetBusyGrabingWeapon( bool busy)
    {
        busyGrabingWeapon = busy;
        animator.SetBool("BusyGrabingWeapon", busyGrabingWeapon);
        Debug.Log(busyGrabingWeapon);
    }
}
