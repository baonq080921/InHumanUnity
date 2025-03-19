using UnityEngine;

public class Player : MonoBehaviour
{

    public ThirdPersonActionAssets controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        controls = new ThirdPersonActionAssets();
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
