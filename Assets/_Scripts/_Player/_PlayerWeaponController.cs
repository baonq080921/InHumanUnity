using UnityEngine;

public class _PlayerWeaponController : MonoBehaviour
{

    private Player player;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 2f;
    private Animator animator;
    [SerializeField] Transform pistolGunPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform aim;
    void Start()
    {
        
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        player.controls.Player.Fire.performed += ctx => Shoot();
    }


    void Shoot(){
        
        Debug.Log("Shoot");
        animator.SetTrigger("isShooting");
        GameObject bullet = Instantiate(bulletPrefab,pistolGunPoint.position,Quaternion.LookRotation(pistolGunPoint.forward));  
        bullet.GetComponent<Rigidbody>().linearVelocity = bulletDirection() * bulletSpeed ; 

        Destroy(bullet,10f);
    }


    // Bullet Direction Calculation
    private Vector3 bulletDirection(){
        Vector3 direction = (aim.position - pistolGunPoint.position).normalized;
        if(!player.aim.CanAimPrecisely()) direction.y = 0;
        weaponHolder.LookAt(aim.position);
        pistolGunPoint.LookAt(aim.position);
        return direction;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(weaponHolder.position,weaponHolder.position + weaponHolder.forward * 25f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pistolGunPoint.position,pistolGunPoint.position + bulletDirection() * 10f);
    }

}
