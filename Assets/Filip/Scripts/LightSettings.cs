using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightSettings : MonoBehaviour
{

    public Light light;
    
    // Start is called before the first frame update
    void Start()
    {
        light.shadows = LightShadows.Soft;
        light.shadowNormalBias = 0f; 
        light.shadowNearPlane = 1f;
        light.shadowBias = 0f;
        QualitySettings.shadowNearPlaneOffset = 1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
