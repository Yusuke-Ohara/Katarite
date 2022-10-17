using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gobletBroke : MonoBehaviour
{
    // Start is called before the first frame update

    float time = 5;
    float nowTime;
    void Start()
    {
        nowTime=0;
    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime > time)Destroy(this.gameObject);
    }
}
