using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{

     private WeaponVisualController weaponVisualController;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponVisualController = GetComponentInParent<WeaponVisualController>();
    }

    public void ReloadIsOver(){
        weaponVisualController.ReturnRigWeightOne();
        Debug.Log("Reload is Over Function Running");
    }

    public void GrabIsOver()
    {
        Debug.Log("Grab is OVer Function Running");
        weaponVisualController.SetBusyGrabingWeapon(false);
    }

    public void RigIncrease()
    {
        weaponVisualController.ReturnRigWeightOne();
        weaponVisualController.ReturnIkWeightOne();
    }
}
