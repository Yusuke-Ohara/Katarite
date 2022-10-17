using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{
    public float raceTime;
    public int boomNum;

    public bool raceBegin;

    public GameObject TimerCanvas;


    // Start is called before the first frame update
    void Start()
    {
        raceTime = 0;
        boomNum = 0;
        raceBegin = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(raceBegin) {
            TimerCanvas.SetActive(true);
            raceTime += Time.deltaTime;
        }
    }

    public void SaveResult(){
        PlayerPrefs.SetFloat("time", raceTime);
        PlayerPrefs.SetInt("boomNum", boomNum);
        PlayerPrefs.Save();
        raceBegin = false;
    }
}
