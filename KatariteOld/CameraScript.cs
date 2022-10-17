using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.XR.XRSettings.showDeviceView = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))transform.Rotate(0,-2,0);
        else if(Input.GetKey(KeyCode.D))transform.Rotate(0,2,0);
    }
}
