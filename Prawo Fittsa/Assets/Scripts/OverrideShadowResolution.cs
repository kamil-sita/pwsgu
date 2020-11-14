using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideShadowResolution : MonoBehaviour
{
    public int shadowRes = 2048;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Light>().shadowCustomResolution = shadowRes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
