using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class WeaponVisualController : MonoBehaviour
{

    [Header("InputSystem")]
    private ThirdPersonActionAssets controls;
    private Player player;
    private Rig rig;

    private Animator animator;
    [Header("GunType")]
    [SerializeField] Transform[] gunTransfom;
    [SerializeField] Transform pistolTransform;
    [SerializeField] Transform rifleTransform;
    [SerializeField] Transform revolverTransform;
    [SerializeField] Transform shotGunTransform;
    [SerializeField] Transform sinperTransform;
    private Transform currentGun;



    [Header("Left Hand")]
    [SerializeField]Transform leftHand;
    [SerializeField] TwoBoneIKConstraint leftHand_IK;
    private enum weapongGrab{sideGrab, backGrab};
    private bool leftHand_IKShouldIncrease;
    private bool busyGrabingWeapon;
    [SerializeField] float leftHandIK_IncreaseStep  = 0.5f;




    [Header("Reload")]
    private bool rigShouldIncrease;
    [SerializeField] float rigIncreaseStep = 0.5f;    

   

    void Start()
    {
        player = GetComponent<Player>();
        rig = GetComponentInChildren<Rig>();
        controls = player.controls;
        controls.Player.Reload.performed += ctx =>{
            if(busyGrabingWeapon == false){
            animator.SetTrigger("isReloading");
            ReduceRig();
            }
        };
        animator = GetComponentInChildren<Animator>();
        EnableGunVisual(pistolTransform);
        SwithAnimatorLayer(1);

    }

    void Update()
    {
        CheckWeapongSwitch();
        UpdateRig();
        UpdateLeftHand_IK();
        
    }

    void UpdateRig(){
        if(rigShouldIncrease){
            rig.weight = Mathf.MoveTowards(rig.weight,1f,rigIncreaseStep * Time.deltaTime) ;
        }
        if(rig.weight >= 1f){
            rigShouldIncrease = false;
        }
    }
    public void ReturnRigWeightOne() => rigShouldIncrease = true;

    public void ReturnIkWeightOne() => leftHand_IKShouldIncrease = true;

    //Checking Weapong Switching toggle between keycap {1,2,3,4}
    private void CheckWeapongSwitch()
    {
        Dictionary<KeyCode,(Transform, int, weapongGrab)> keyInputMap = new Dictionary<KeyCode, (Transform,int,weapongGrab)>{
            {KeyCode.Alpha1,(pistolTransform,1,weapongGrab.sideGrab)},
            {KeyCode.Alpha2,(rifleTransform,1,weapongGrab.backGrab)},
            {KeyCode.Alpha3,(revolverTransform,1,weapongGrab.sideGrab)},
            {KeyCode.Alpha4,(shotGunTransform,2,weapongGrab.backGrab)},
            {KeyCode.Alpha5,(sinperTransform,3,weapongGrab.backGrab)},
        };

        foreach(var entry in keyInputMap){
            if(Input.GetKeyDown(entry.Key)){
                EnableGunVisual(entry.Value.Item1);
                SwithAnimatorLayer(entry.Value.Item2);
                SwitchingGun(entry.Value.Item3);
                break;
            }
        }
    }


    // Checking Gun Enable and Disable Gun Visual
    void EnableGunVisual(Transform gun){
        DisableGunVisual();
        gun.gameObject.SetActive(true);
        currentGun = gun;
       LeftHandTargetTransform();
    }


    void DisableGunVisual(){
        foreach(Transform item in gunTransfom){
            item.gameObject.SetActive(false);
        }
    }


    // Hand Transfromation for each weapon correspoding
    void LeftHandTargetTransform(){
        Transform leftHandGunTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;
        leftHand.localPosition = leftHandGunTransform.localPosition;
        leftHand.localRotation = leftHandGunTransform.localRotation;
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

    void UpdateLeftHand_IK()
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
        animator.SetBool("isGrabing", busyGrabingWeapon);
        Debug.Log(busyGrabingWeapon);
    }
}
