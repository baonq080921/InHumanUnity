using Unity.VisualScripting;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] Transform[] gunTransfom;
    [SerializeField] Transform pistolTransform;
    [SerializeField] Transform rifleTransform;
    [SerializeField] Transform revolverTransform;
    [SerializeField] Transform shotGunTransform;
    [SerializeField] Transform sinperTransform;


    private Transform currentGun;
    [Header("Left Hand")]
    [SerializeField]Transform leftHand;
    
    void Start()
    {
        EnableGunVisual(pistolTransform);
        
    }
   

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            EnableGunVisual(pistolTransform);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            EnableGunVisual(rifleTransform);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            EnableGunVisual(revolverTransform);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            EnableGunVisual(shotGunTransform);
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
    }


    void DisableGunVisual(){
        foreach(Transform item in gunTransfom){
            item.gameObject.SetActive(false);
        }
    }

    void LeftHandTargetTransform(){
        Transform leftHandGunTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;
        leftHand.position = leftHandGunTransform.position;
        leftHand.rotation = leftHandGunTransform.rotation;
    }

}
