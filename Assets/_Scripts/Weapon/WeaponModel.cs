using UnityEngine;

public enum weapongGrab{sideGrab, backGrab};
public enum HoldType{CommonHold = 1, LowHold = 2, HighHold = 3};

public class WeaponModel : MonoBehaviour
{

    public WeaponType weaponType; // Enum to define the type of weapon
    public weapongGrab weapongGrab; // Enum to define the type of weapon grab
    public HoldType holdType; // Enum to define the type of hold
    public Transform gunPoint;
    public Transform holdPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
