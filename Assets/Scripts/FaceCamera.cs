using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FaceCamera : MonoBehaviour
{
    public Transform CameraTransform;
    
    void Update()
    {
        transform.LookAt(CameraTransform);
    }
}
