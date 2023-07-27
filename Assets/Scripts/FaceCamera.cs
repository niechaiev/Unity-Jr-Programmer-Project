using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform camera;

    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera);
    }
}
