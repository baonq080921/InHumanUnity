using UnityEngine;

public class CameraFollowinng : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float followSpeed = 1f;
    [SerializeField] float yOffSet = 5f;
    [SerializeField] float xOffset = 5f;
    [SerializeField] float zOffset = 5f;


    //Updating Camera follow player
    void LateUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x + xOffset,target.position.y + yOffSet, target.position.z + zOffset);
        this.transform.position = Vector3.Lerp(this.transform.position,newPos,followSpeed * Time.deltaTime);
    }
}

