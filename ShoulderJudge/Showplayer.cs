using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showplayer : MonoBehaviour
{
    public float[] savedata = new float[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}; 

    // Start is called before the first frame update
    void Start()
    {
        savedata[0] = PlayerPrefs.GetFloat("MaxR_x",0);
        savedata[1] = PlayerPrefs.GetFloat("MaxR_y",0);
        savedata[2] = PlayerPrefs.GetFloat("MaxR_z",0);
        savedata[3] = PlayerPrefs.GetFloat("MinR_x",0);
        savedata[4] = PlayerPrefs.GetFloat("MinR_y",0);
        savedata[5] = PlayerPrefs.GetFloat("MinR_z",0);
        savedata[6] = PlayerPrefs.GetFloat("MaxL_x",0);
        savedata[7] = PlayerPrefs.GetFloat("MaxL_y",0);
        savedata[8] = PlayerPrefs.GetFloat("MaxL_z",0);
        savedata[9] = PlayerPrefs.GetFloat("MinL_x",0);
        savedata[10] = PlayerPrefs.GetFloat("MinL_y",0);
        savedata[11] = PlayerPrefs.GetFloat("MinL_z",0);
        savedata[12] = PlayerPrefs.GetFloat("avenormalR_x", 0);
        savedata[13] = PlayerPrefs.GetFloat("avenormalR_y", 0);
        savedata[14] = PlayerPrefs.GetFloat("avenormalR_z", 0);
        savedata[15] = PlayerPrefs.GetFloat("avenormalL_x", 0);
        savedata[16] = PlayerPrefs.GetFloat("avenormalL_y", 0);
        savedata[17] = PlayerPrefs.GetFloat("avenormalL_z", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
