using UnityEngine;

public class _PlayerWeaponController : MonoBehaviour
{

    private Player player;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 2f;
    private Animator animator;
    [SerializeField] Transform pistolGunPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        player.controls.Player.Fire.performed += ctx => Shoot();
    }


    void Shoot(){
        Debug.Log("Shoot");
        animator.SetTrigger("isShooting");
        GameObject bullet = Instantiate(bulletPrefab,pistolGunPoint.position,pistolGunPoint.rotation);
        bullet.GetComponent<Rigidbody>().linearVelocity = pistolGunPoint.forward * bulletSpeed; 

        Destroy(bullet,1f);
    }

}
