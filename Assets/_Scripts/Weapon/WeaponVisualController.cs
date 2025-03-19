using System;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponVisualController : MonoBehaviour
{
    public float input;
    private ThirdPersonActionAssets controller;
    [SerializeField] Transform [] gunTransfrom;
    [SerializeField] Transform pistolTransform;
    [SerializeField] Transform riffleTransform;
    [SerializeField] Transform shotGunTransform;
    [SerializeField] Transform revolverTransform;
    [SerializeField] Transform sniperTransfrom;
    void Awake()
    {
        controller = new ThirdPersonActionAssets();
controller.Player.Weapon.performed += ctx => input = ctx.ReadValue<float>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        SwithOffGun();
    }

    void OnEnable()
    {
        controller.Enable();
    }
    void OnDisable()
    {
        controller.Disable();
    }

    // Update is called once per frame
   void Update()
{
    int weaponIndex = Mathf.RoundToInt((float)input); // Chuyển từ double -> int
    Debug.Log("Current Input: " + weaponIndex);

    switch (weaponIndex)
    {
        case 1:
            SwithWeapon(pistolTransform);
            break;
        case 2:
            SwithWeapon(riffleTransform);
            break;
        case 3:
            SwithWeapon(shotGunTransform);
            break;
        case 4:
            SwithWeapon(revolverTransform);
            break;
        case 5:
            SwithWeapon(sniperTransfrom);
            break;
        default:
            Debug.LogWarning("Invalid weapon input: " + input);
            break;
    }
}


    void SwithWeapon(Transform gun){
       gun.gameObject.SetActive(true);
    }
    void SwithOffGun(){

        for(int i = 0 ; i < gunTransfrom.Length; i++){
            gunTransfrom[i].gameObject.SetActive(false);
        }
    }
}
