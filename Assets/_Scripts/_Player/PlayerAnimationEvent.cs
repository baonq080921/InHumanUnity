using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{

     private WeaponVisualController weaponVisualController; 
     private _PlayerWeaponController weaponController; // Reference to the Weapon class      


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponVisualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver(){
        weaponVisualController.ReturnRigWeightOne();
        weaponVisualController.PlayReloadAnimation();
        // Debug.Log("Reload is Over Function Running");
    }

    public void GrabIsOver()
    {
        // Debug.Log("Grab is OVer Function Running");
        weaponVisualController.SetBusyGrabingWeapon(false);
    }

    public void RigIncrease()
    {
        weaponVisualController.ReturnRigWeightOne();
        weaponVisualController.ReturnIkWeightOne();
    }
    public void PlayAnimation(){
        weaponVisualController.SwitchOnWeaponModel();
        Debug.Log("Play Animation Function Running");
    }
}
