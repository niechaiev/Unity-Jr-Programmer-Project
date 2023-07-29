using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform CameraTransform;
    
    void Update()
    {
        var position = transform.position;
        transform.LookAt(new Vector3(CameraTransform.position.x, position.y, position.z));
    }
}
