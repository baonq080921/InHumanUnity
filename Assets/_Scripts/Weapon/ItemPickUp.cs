using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<_PlayerWeaponController>() != null)
        {
           _PlayerWeaponController playerWeaponController = other.GetComponent<_PlayerWeaponController>();
           if(playerWeaponController.weaponSlot.Count < playerWeaponController.maxSlotAllowed)
           {
               playerWeaponController.weaponSlot.Add(weapon);
               Destroy(gameObject); // Destroy the pickup object after adding the weapon to the inventory
           }
           else
           {
               Debug.Log("Inventory full! Cannot pick up more weapons.");
           }
        }
    }
}
