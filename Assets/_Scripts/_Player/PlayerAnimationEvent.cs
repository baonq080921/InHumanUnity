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
    }
}
