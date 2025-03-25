using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{

    [Header("InputSystem")]
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



    [Header("Reload")]
    [SerializeField] float rigIncreaseRate = .1f; 
    private bool rigShouldIncrease;
    

    [Header("Left Hand")]
    [SerializeField] TwoBoneIKConstraint leftHand_IK;
    [SerializeField] Transform leftHandIK_Target;
    [SerializeField] bool shouldIncreaseLeftHandIK;    
    [SerializeField] float leftHandIK_IncreaseRate = .1f ;
    private enum GrabType{sideGrab, backGrab};
    private bool busyGrabbingWeapon;



    void Start()
    {
        player = GetComponent<Player>();
        rig = GetComponentInChildren<Rig>();
        animator = GetComponentInChildren<Animator>();
        EnableGunVisual(pistolTransform);
        SwithAnimatorLayer(1);

    }

    void Update()
    {
        CheckWeaponSwitch();
        if(Input.GetKeyDown(KeyCode.R) && busyGrabbingWeapon == false){
            animator.SetTrigger("isReloading");
            ReduceWeight();
        }
        UpdateRig();
        UpdateLeftHand_IK();
        
    }

    void UpdateRig()
    {
        if (rigShouldIncrease)
        {
            rig.weight = Mathf.MoveTowards(rig.weight,1f,rigIncreaseRate * Time.deltaTime) ;
        }
        if (rig.weight >= 1f)
        {
            rigShouldIncrease = false;
            Debug.Log("Rig increase " + rigShouldIncrease);
        }

    }

    private void UpdateLeftHand_IK()
    {
        if (shouldIncreaseLeftHandIK)
        {
            leftHand_IK.weight = Mathf.MoveTowards(leftHand_IK.weight,1f,leftHandIK_IncreaseRate * Time.deltaTime);
        }
        if (leftHand_IK.weight >= 1f)
        {
            shouldIncreaseLeftHandIK = false;
            Debug.Log("LeftHandIK increase " + shouldIncreaseLeftHandIK);
        }
    }

    public void ReturnRigWeightToOne()=> rigShouldIncrease = true;

    public void ReturnLeftHand_IkWeightToOne() => shouldIncreaseLeftHandIK = true;

    //Checking Weapong Switching toggle between keycap {1,2,3,4}
    private void CheckWeaponSwitch()
{
    if (Input.anyKeyDown)
    {
        Dictionary<KeyCode, (Transform, int, GrabType)> weaponMap = new Dictionary<KeyCode, (Transform, int, GrabType)>
        {
            { KeyCode.Alpha1, (pistolTransform, 1, GrabType.sideGrab) },
            { KeyCode.Alpha2, (rifleTransform, 1, GrabType.backGrab) },
            { KeyCode.Alpha3, (revolverTransform, 1, GrabType.sideGrab) },
            { KeyCode.Alpha4, (shotGunTransform, 2, GrabType.backGrab) },
            { KeyCode.Alpha5, (sinperTransform, 3, GrabType.backGrab) }
        };

        foreach (var entry in weaponMap)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                EnableGunVisual(entry.Value.Item1);
                SwithAnimatorLayer(entry.Value.Item2);
                SwitchingGun(entry.Value.Item3);
                break;
            }
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

    void SwitchingGun(GrabType grabType)
    {
        animator.SetFloat("weaponTypeGrab", (float)grabType);
        animator.SetTrigger("WeaponGrab");
        ReduceLeftHandIK_Weight();
        ReduceWeight();
        SetBusyGrabingWeapon(true);

    }

    private void ReduceLeftHandIK_Weight()
    {
        leftHand_IK.weight = 0f;
    }

    public void SetBusyGrabingWeapon(bool busy)
    {
        busyGrabbingWeapon = busy;
        animator.SetBool("BusyGrabingWeapon", busyGrabbingWeapon);
        Debug.Log(busyGrabbingWeapon);
    }

    private void ReduceWeight()
    {
        rig.weight = 0;
    }

}
