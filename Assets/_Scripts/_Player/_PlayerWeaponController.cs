using UnityEngine;

public class _PlayerWeaponController : MonoBehaviour
{

    private Player player;
    private Animator animator;
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
    }

}
