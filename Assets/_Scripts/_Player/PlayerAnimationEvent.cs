using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{

     private WeaponVisualController weaponVisualController;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponVisualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver(){
        weaponVisualController.ReturnRigWeightToOne();
        Debug.Log("Hello this is reload");
    }

    public void ReturnRig()
    {
        Debug.Log("Hello this is Return Rig Function");
        weaponVisualController.ReturnLeftHand_IkWeightToOne();
        weaponVisualController.ReturnRigWeightToOne();
    }

    public void WeaponGrabingIsOver(){
        weaponVisualController.SetBusyGrabingWeapon(false);
        Debug.Log("Hello World");
    }
}
