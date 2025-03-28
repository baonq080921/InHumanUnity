using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // automatically add a Rigidbody component if not present
public class Target : MonoBehaviour
{


    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy"); // set the layer to "Enemy"

    }
}
