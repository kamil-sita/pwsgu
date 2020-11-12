using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    //bazowany na https://forum.unity.com/threads/simple-rotation-script-free.510303/
    [Range(-1.0f, 1.0f)]
    public float xForceDirection = 0.0f;
    [Range(-1.0f, 1.0f)]
    public float yForceDirection = 0.0f;
    [Range(-1.0f, 1.0f)]
    public float zForceDirection = 0.0f;

    public float speedMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(xForceDirection * speedMultiplier,
                           yForceDirection * speedMultiplier,
                           zForceDirection * speedMultiplier,
                           Space.Self);
    }
}
