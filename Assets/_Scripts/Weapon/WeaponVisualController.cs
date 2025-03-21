using Unity.VisualScripting;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{

    private Animator animator;
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
    
    void Start()
    {

        animator = GetComponentInParent<Animator>();
        EnableGunVisual(pistolTransform);
        SwithAnimatorLayer(1);
        
    }
   

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {

            EnableGunVisual(pistolTransform);
            SwithAnimatorLayer(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            EnableGunVisual(rifleTransform);
            SwithAnimatorLayer(1);

        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            EnableGunVisual(revolverTransform);
            SwithAnimatorLayer(1);

        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            EnableGunVisual(shotGunTransform);
            SwithAnimatorLayer(2);

        }
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            EnableGunVisual(sinperTransform);
        }
        
    }

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

}
