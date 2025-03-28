using UnityEngine;
 public enum WeaponType
    {
        Pistol,
        AutoRiffle,
        Revolver,
        Shotgun,
        SniperRiffle,
    }
   

[System.Serializable] // This is a class that can be serialized by Unity, allowing it to be saved and loaded in the editor.
public class Weapon
{

    public WeaponType weaponType;

    public int bulletsInClip; // The number of bullets currently in the clip.
    public int clipCapacity; // The maximum number of bullets that can be held in the clip.
    public int totalReserAmmo; // The total amount of ammo available for reloading.

    public bool CanFire()
    {
        return HaveEnoughBullet();
    }

    private bool HaveEnoughBullet()
    {
        if (bulletsInClip > 0)
        {
            bulletsInClip--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanReload()
    {
        // Cant reload if the clip is full or if there is no ammo left to reload.
        if(bulletsInClip >= clipCapacity) return false;
        bool reload = totalReserAmmo > 0 ? true : false;
        return reload;
    }

    public void RefillBullet(){
        //Reback the bullet in the total reserve ammo.
        //totalReserAmmo += bulletsInClip;

        int bulletToReload = clipCapacity;
        if(bulletToReload > totalReserAmmo) bulletToReload = totalReserAmmo;

        totalReserAmmo -= bulletToReload;
        bulletsInClip  = bulletToReload;

        if(totalReserAmmo < 0) totalReserAmmo = 0;
    }
}
