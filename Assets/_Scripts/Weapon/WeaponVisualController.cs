using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
    [SerializeField] Transform rightHand;

    [Header("Reload")]
    private bool isReloading;
    [SerializeField] float ReloadSmoothSpeed = 0.5f;    

    [Header("Grab")]
    [SerializeField] TwoBoneIKConstraint leftHand_IK;
    [SerializeField] TwoBoneIKConstraint rightHand_IK;
    private enum weapongGrab{sideGrab, backGrab};
    private bool grabGunShouldIncrease;
    [SerializeField] float smoothGrabSpeed  = 0.5f;



    void Start()
    {
        isReloading = true;
        grabGunShouldIncrease = true;
        player = GetComponent<Player>();
        rig = GetComponentInChildren<Rig>();
        controls = player.controls;
        controls.Player.Reload.performed += ctx =>{
            animator.SetTrigger("isReloading");
            rig.weight = 0f;
        };
        animator = GetComponentInChildren<Animator>();
        EnableGunVisual(pistolTransform);
        SwithAnimatorLayer(1);

    }

    void Update()
    {
        CheckWeapongSwitch();
        CheckReload();
        CheckGrabGunBack();
        
    }

    void CheckReload(){
        if(isReloading){
            rig.weight += ReloadSmoothSpeed * Time.deltaTime;
        }
        if(rig.weight > 1f){
            isReloading = false;
        }
    }

    public void ReturnRigWeightOne() {
        isReloading = true;
        }

    //Checking Weapong Switching toggle between keycap {1,2,3,4}
    private void CheckWeapongSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            EnableGunVisual(pistolTransform);
            SwithAnimatorLayer(1);
            SwitchingGun((float)weapongGrab.sideGrab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnableGunVisual(rifleTransform);
            SwithAnimatorLayer(1);
            SwitchingGun((float)weapongGrab.backGrab);


        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EnableGunVisual(revolverTransform);
            SwithAnimatorLayer(1);
            SwitchingGun((float)weapongGrab.sideGrab);


        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EnableGunVisual(shotGunTransform);
            SwithAnimatorLayer(2);
            SwitchingGun((float)weapongGrab.backGrab);


        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            EnableGunVisual(sinperTransform);
            SwithAnimatorLayer(3);
            SwitchingGun((float)weapongGrab.backGrab);

        }
    }


    // Checking Gun Enable and Disable Gun Visual
    void EnableGunVisual(Transform gun){
        DisableGunVisual();
        gun.gameObject.SetActive(true);
        currentGun = gun;
       LeftHandTargetTransform();
       RightHandTargetTranform();
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

    void RightHandTargetTranform(){
        Transform rightHandGunTransform = currentGun.GetComponentInChildren<RightHandTargetTransform>().transform;
        if(rightHandGunTransform == null){
            return;
        }
        rightHand.localPosition = rightHandGunTransform.localPosition;
        rightHand.localRotation = rightHandGunTransform.localRotation;
    }

    // Switch the layer of the Animator with the index from 1 to n;
    void SwithAnimatorLayer(int layerIndex){
        for(int i = 1; i < animator.layerCount; i++){
            animator.SetLayerWeight(i,0);
        }
        animator.SetLayerWeight(layerIndex,1);
    }

    void SwitchingGun(float weaponGrabFloat){
        animator.SetFloat("weaponTypeGrab",weaponGrabFloat);
        animator.SetTrigger("WeaponGrab");
        leftHand_IK.weight = 0.15f;
        rightHand_IK.weight = 0.15f;
    }

    void CheckGrabGunBack(){
        if(grabGunShouldIncrease){
             leftHand_IK.weight += smoothGrabSpeed * Time.deltaTime;
            rightHand_IK.weight += smoothGrabSpeed * Time.deltaTime;
        }
        if(leftHand_IK.weight > 1f && rightHand_IK.weight > 1f){
            grabGunShouldIncrease = false;
        }
    }

}
