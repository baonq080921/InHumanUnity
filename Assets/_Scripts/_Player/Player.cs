using UnityEngine;

public class Player : MonoBehaviour
{

    public ThirdPersonActionAssets controls{get; private set;}
    public PlayerAim aim { get; private set; }  
    public _Player_Movement movement { get; private set; }  
    public _PlayerWeaponController weapon{get; private set;}   
    public WeaponVisualController weaponVisualController{get; private set;} 
    public WeaponModel weaponModel{get; private set;} // This is the current weapon model that is being used by the player.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        controls = new ThirdPersonActionAssets();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<_Player_Movement>();
        weapon = GetComponent<_PlayerWeaponController>();   
        weaponVisualController = GetComponent<WeaponVisualController>();    
    }
        

    void OnEnable()
    {
        controls.Enable();
    }
    void OnDisable()
    {
        controls.Disable();
    }
}
