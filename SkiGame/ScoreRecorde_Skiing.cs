using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecorde_Skiing : MonoBehaviour
{
    private bool raceBeginSkiing;

    private int flagNumSkiing;//旗ポイント
    private int flagCount = 30;
    private float raceTimeSkiing;//経過時間 
    private int jumpNumSkiing;//ジャンプのポイント

    [SerializeField]
    private float jumpTime; // 全体のエアタイム
    private bool onJump; //ジャンプ中か否かのフラグ
    private float jumpTimeTmp; //1回のジャンプのエアタイム
    
    [SerializeField]
    private GameObject airTimeLabel;
    [SerializeField]
    private Text airTimeText;

    [SerializeField]
    private int flagpoint = 10;
    [SerializeField]
    private int normalJumpPoint = 10;//普通のジャンプのポイント
    [SerializeField]
    private int spinAfterJumpPoint = 20;//ジャンプした後回転したときのポイント
    [SerializeField]
    private int horizontalSpinWithJumpPoint = 50;//ジャンプ＋横回転を同時にしたときのポイント
    [SerializeField]
    private int verticalSpinWithJumpPoint = 30;//ジャンプ＋縦回転を同時にした時のポイント

    // Start is called before the first frame update
    void Start()
    {
        raceBeginSkiing = false;
        onJump = false;
        flagNumSkiing = 0;
        raceTimeSkiing = 0.0f;
        jumpNumSkiing = 0;
        jumpTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(raceBeginSkiing){
            raceTimeSkiing += Time.deltaTime;
        }

        if(onJump){
            jumpTimeTmp += Time.deltaTime;
            airTimeText.text = jumpTimeTmp.ToString("f2");
        }

    }

    public void RaceStart(){//レース開始時処理
        raceBeginSkiing = true;
    }

    public void NormalJump(){//通常ジャンプポイント加算
        jumpNumSkiing += normalJumpPoint;
    }

    public void SpinAfterJump(){//ジャンプ後回転ポイント加算
        jumpNumSkiing += spinAfterJumpPoint;
    }

    public void HorizontalSpinWithJump(){//ジャンプ＋横回転同時の際のポイント加算
        jumpNumSkiing += horizontalSpinWithJumpPoint;
    }

    public void VerticalSpinWithJump(){//ジャンプ＋縦回転同時の際のポイント加算
        jumpNumSkiing += verticalSpinWithJumpPoint;
    }

    public void flagPassage(){//旗を通過した際のポイント加算
        flagNumSkiing++;
        flagCount -= 1;
    }


    public void JumpStart(){
        onJump = true;
        jumpTimeTmp = 0;
        airTimeLabel.SetActive(true);
    }

    public void JumpEnd(){
        onJump = false;
        airTimeLabel.SetActive(false);
        jumpTime += jumpTimeTmp;
        jumpTimeTmp = 0;
    }
    public void SaveResult(){//得点セーブ
        PlayerPrefs.SetFloat("timeSkiing",raceTimeSkiing);
        PlayerPrefs.SetInt("flagsPoint",flagNumSkiing);
        PlayerPrefs.SetInt("flagcount",flagCount);
        PlayerPrefs.SetInt("jumpPoint",jumpNumSkiing);
        PlayerPrefs.SetFloat("jumpTimeNum",jumpTime);
        PlayerPrefs.Save();
    }
}

