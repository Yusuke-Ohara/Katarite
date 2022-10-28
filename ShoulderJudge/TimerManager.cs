using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// プレイ画面に表示するタイマーの制御
public class TimerManager : MonoBehaviour
{

    public GameObject player;
    ScoreRecorder sr;
    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        sr = player.GetComponent<ScoreRecorder>();
        timerText.text = "00:00";
    }

    // Update is called once per frame
    void Update()
    {
        if(sr.raceBegin){
            float minutes = Mathf.FloorToInt(sr.raceTime / 60);
            float seconds = Mathf.FloorToInt(sr.raceTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    
}
