using UnityEngine;

public class TestingHitting : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }


    void OnCollisionEnter(Collision collision)
    {
       rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
