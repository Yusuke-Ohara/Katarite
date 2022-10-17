using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSpeed : MonoBehaviour
{
    ShoulderDriveSimple sds;
    public GameObject speedLabelObj;
    Text speedLabel;

    // Start is called before the first frame update
    void Start()
    {
        sds = GetComponent<ShoulderDriveSimple>();
        speedLabel = speedLabelObj.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        speedLabel.text = "Speed : " + sds.nowSpeed;
    }
}
