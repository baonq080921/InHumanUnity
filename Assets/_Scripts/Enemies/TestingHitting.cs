using UnityEngine;

public class TestingHitting : MonoBehaviour
{
    [SerializeField] Material gotHitMaterial;
    [SerializeField] Material healMaterial;


    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet"){
            GetComponent<MeshRenderer>().material = gotHitMaterial;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        GetComponent<MeshRenderer>().material = healMaterial;
    }
}
