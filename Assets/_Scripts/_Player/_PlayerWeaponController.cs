using System.Collections.Generic;
using UnityEngine;

public class _PlayerWeaponController : MonoBehaviour
{


    private const float REFERENCE_BULLET_SPEED = 20f; // THis is a default value for the bullet speed to caculate the mass

    private Player player;


    private Animator animator;
    

    [Header("Bullet details")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 2f;
    [SerializeField] Transform pistolGunPoint;
    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] public List<Weapon> weaponSlot;

    public int maxSlotAllowed = 2; // The maximum number of weapons that can be held in the inventory  

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {

        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        AssignInputEvent();
        currentWeapon.bulletsInClip = currentWeapon.totalReserAmmo;
    }

    


#region Slot-management -PickUp/Drop/PickUpWeapon/EquipWeapon
    // This function is used to equip a weapon from the inventory based on the slot index.
    // The slot index is passed as an argument to the function.
    // The current weapon is set to the weapon in the specified slot index.
    // If the slot index is out of range, it does nothing.

     void EquipWeapon(int slotIndex){
        Debug.Log("weapon slot: " + slotIndex);
        player.weaponVisualController.SwitchOnWeaponModel();
        currentWeapon = weaponSlot[slotIndex];
    }

    void DropCurrentWeapon(){
        if(weaponSlot.Count <= 1) return;

        weaponSlot.Remove(currentWeapon);
        if(weaponSlot.Count > 0) currentWeapon = weaponSlot[0];
        else currentWeapon = null;
    }

    public void PickUpWeapon(Weapon newWeapon){
        if(weaponSlot.Count >= maxSlotAllowed){
            return;
        }
        weaponSlot.Add(newWeapon);
    }
    
#endregion
   

    void Shoot(){

       if(currentWeapon.CanFire() == false) return; // Check if the player has enough ammo to shoot        
        Debug.Log("Shoot");
        animator.SetTrigger("isShooting");
        GameObject bullet = Instantiate(bulletPrefab,pistolGunPoint.position,Quaternion.LookRotation(pistolGunPoint.forward));
        Rigidbody rbNewBullet = bullet.GetComponent<Rigidbody>();  
        // caculate the new mass for the bullet when the bullet speed is changing
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed; 

        rbNewBullet.linearVelocity = bulletDirection() * bulletSpeed ; 
        Destroy(bullet,10f);
    }


    public Weapon CurrentWeapon(){
        return currentWeapon;
    }
    // Bullet Direction Calculation
    public Vector3 bulletDirection(){
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - pistolGunPoint.position).normalized;
        if(!player.aim.CanAimPrecisely() && player.aim.Target() == null) direction.y = 0;
        // weaponHolder.LookAt(aim.position);
        // pistolGunPoint.LookAt(aim.position);
        return direction;
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(weaponHolder.position,weaponHolder.position + weaponHolder.forward * 25f);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawLine(pistolGunPoint.position,pistolGunPoint.position + bulletDirection() * 10f);
    // }


    public Transform GunPoint() => pistolGunPoint;



    #region Input Event
        
    void AssignInputEvent()
    {
        player.controls.Player.Fire.performed += ctx => Shoot();
        player.controls.Player.EquipSlot1.performed += ctx => EquipWeapon(0);
        player.controls.Player.EquipSlot2.performed += ctx => EquipWeapon(1);
        player.controls.Player.DropCurrentWeapon.performed += ctx => DropCurrentWeapon();
        player.controls.Player.Reload.performed += ctx =>{

            if(currentWeapon.CanReload()){
                player.weaponVisualController.PlayReloadAnimation();
            }
        };

    }
    #endregion
}
