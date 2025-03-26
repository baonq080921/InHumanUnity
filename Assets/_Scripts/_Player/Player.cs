using UnityEngine;

public class Player : MonoBehaviour
{

    public ThirdPersonActionAssets controls{get; private set;}
    public PlayerAim aim { get; private set; }  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        controls = new ThirdPersonActionAssets();
        aim = GetComponent<PlayerAim>();
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
