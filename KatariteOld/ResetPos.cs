using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    public Vector3 respawnPos;
    // Start is called before the first frame update
    void Start()
    {
        respawnPos = transform.position + new Vector3(0,4,0);;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            transform.position = respawnPos;
        }
    }
}
