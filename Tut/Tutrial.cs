using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


// ShoulderDriveのチュートリアル
public class Tutrial : MonoBehaviour
{
    public GameObject TutCan; //チュートリアルキャンバス
    public Text TutText; //チュートリアルキャンバスのタイトルテキスト
    
    private int vs; //ビデオの状態（0：再生前，1：再生中，2：終了，3：シーン開始時）
    [SerializeField]
    private int videoNum; //シーンの状態

    private bool exp; //説明時かどうか（true：説明，false：練習）
    private int count; //ビデオのループ回数

    ShoulderDriveSimple sds;
    Example exm;
    Respown rsp;
    Calibration cal;
    Rigidbody rb;
    private Vector3 vel;
    private bool col;
    public GameObject bgm;

    // キャリブレーション 0
    public GameObject calibration;
    private int calNum;
    private bool relaxFlag;
    public GameObject relax;
    public GameObject move;
    private bool judgeFlag;
    public GameObject sousa;
    private int LoadNum;


    // ハンドル 1
    public GameObject hundle;
    public GameObject ExpHun;
    public VideoPlayer ExpAvaHun;
    public GameObject TutHun;
    public VideoPlayer TutAvaHun;
    public GameObject HundleCam;
    public GameObject TopCam;


    // ジャンプ 2
    public GameObject jump;
    public GameObject ExpJum;
    public VideoPlayer ExpAvaJum;
    public GameObject TutJum;
    public VideoPlayer TutAvaJum;
    public Text JumpTiming;
    public GameObject GoodJump;
    public GameObject CnsShader;


    // ワイパー 3
    public GameObject wiper;
    public GameObject ExpWip;
    public VideoPlayer ExpAvaWip;
    public GameObject TutWip;
    public VideoPlayer TutAvaWip;
    public VideoClip wiperR;
    public VideoClip wiperL;
    public GameObject FrontRaw;
    public Text WiperText;
    public GameObject GoodWip;
    private bool wiperTut;
    

    //アイテムギミック 4&5
    public GameObject item;
    public GameObject ExpItem;
    public GameObject TutItem;
    public RawImage Goblet;
    public VideoPlayer GobVideo;
    public GameObject GobText;
    public RawImage Dash;
    public VideoPlayer DashVideo;
    public GameObject DashText;
    private float time;
    private bool itemFlag;

    public GameObject gameStart;

    // カウントダウン　7
    public GameObject WipCam;
    public Text countDown;





    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        bgm.GetComponent<AudioSource>().volume = 0.2f;
        sds = this.GetComponent<ShoulderDriveSimple>();
        exm = this.GetComponent<Example>();
        rsp = this.GetComponent<Respown>();
        cal = TutCan.GetComponent<Calibration>();
        rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            ChangeVideoNum();
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            TutCan.SetActive(true);
            calibration.SetActive(false);
            hundle.SetActive(false);
            jump.SetActive(false);
            wiper.SetActive(false);
            gameStart.SetActive(false);
            item.SetActive(false);
            HundleCam.SetActive(false);
            TopCam.SetActive(false);
            CnsShader.SetActive(false);
            WipCam.SetActive(false);
            rsp.RespownPos(rsp.startpoint);
            videoNum = 6;
        }

        // キャリブレーション
        if(videoNum == 0){
            calibration.SetActive(true);
            TutText.text = "準備運動(肩の動き計測中)";
            if(calNum == 0){
                relax.SetActive(true);
                move.SetActive(false);
                sousa.SetActive(false);
                relaxFlag = cal.Relax;
                if(relaxFlag){
                    calNum = 1;
                }
            }
            if(calNum == 1){
                relax.SetActive(false);
                move.SetActive(true);
                judgeFlag = cal.calend;
                if(judgeFlag){
                    move.SetActive(false);
                    time += Time.deltaTime;
                    TutText.text = "操作方法";
                    sousa.SetActive(true);
                    if(time > 3)
                    {
                        calibration.SetActive(false);
                        ChangeVideoNum();
                    }
                }
            } 
        }

        // ハンドル操作
        if(videoNum == 1){
            if(LoadNum == 0){
                exm.Load();
                LoadNum++;
            }
            calibration.SetActive(false);
            hundle.SetActive(true);
            HundleCam.SetActive(true);
            TopCam.SetActive(true);
            TutText.text = "ハンドル操作";
            if(exp){
                ExpHun.SetActive(true);
                TutHun.SetActive(false);
                rb.isKinematic = true;
                if(vs == 3){
                    vs = 0;  
                }
                if(vs == 0){
                    VideoStart(ExpAvaHun); 
                }
                if(vs == 1){
                    VideoStop(ExpAvaHun,1);
                }
                if(vs == 2){
                    Debug.Log("ExpEnd");
                    ExpHun.SetActive(false);
                    TutHun.SetActive(true);
                    exp = false;
                    vs = 0;
                }
            } 
            if(!exp){
                exm.hundleflag = true;
                if(vs == 0){
                    TutAvaHun.isLooping = true;
                    TutAvaHun.Play();
                    rb.isKinematic = false;
                    vs = 1;
                }
                if(vs == 2){
                    TutAvaHun.Stop();
                    HundleCam.SetActive(false);
                    TopCam.SetActive(false);
                    col = true;
                    ChangeVideoNum();
                }
            }
        }

        // ジャンプ
        if(videoNum == 2){
            hundle.SetActive(false);
            HundleCam.SetActive(false);
            TopCam.SetActive(false);
            jump.SetActive(true);
            exm.hundleflag = false;
            TutText.text = "ジャンプ";
            if(exp){
                ExpJum.SetActive(true);
                TutJum.SetActive(false);
                if(vs == 3){
                    vs = 0; 
                }
                if(vs == 0){
                    VideoStart(ExpAvaJum);
                    Debug.Log("count " + count); 
                }
                if(vs == 1){
                    VideoStop(ExpAvaJum,1);
                }
                if(vs == 2){
                    Debug.Log("ExpEnd");
                    ExpJum.SetActive(false);
                    TutJum.SetActive(true);
                    exp = false;
                    vs = 0;
                }
            }
            if(!exp){
                if(vs == 0){
                    TutAvaJum.isLooping = true;
                    TutAvaJum.Stop();
                    rb.isKinematic = false;
                    col = true;
                    vs = 1;
                }
                if(vs == 1){
                    if(sds.jumpCom){
                        Debug.Log("Junp");
                        rb.isKinematic = false;
                        sds.CarJump(); 
                        JumpTiming.text = "";
                        GoodJump.SetActive(true);
                        TutAvaJum.Stop();
                    }
                }
                
                if(vs == 2){
                    col = true;
                    ChangeVideoNum();
                }
            }
        }

        // ワイパー
        if(videoNum == 3){
            jump.SetActive(false);
            WipCam.SetActive(true);
            wiper.SetActive(true);
            exm.hundleflag = true;
            TutText.text = "ワイパー";
            if(exp){
                exm.wiperFlag = false;
                ExpWip.SetActive(true);
                TutWip.SetActive(false);
                if(vs == 3){
                    vs = 0;  
                    col = true;
                }
                if(vs == 0){
                    VideoStart(ExpAvaWip);   
                }
                if(vs == 1){
                    VideoStop(ExpAvaWip,1);
                }
                if(vs == 2){
                    Debug.Log("ExpEnd");
                    ExpWip.SetActive(false);
                    TutWip.SetActive(true);
                    exp = false;
                    vs = 0;
                }
            } 
            if(!exp){
                if(vs == 0){
                    TutAvaWip.isLooping = true;
                    TutAvaWip.clip = wiperR;
                    TutAvaWip.Play();
                    time = 0;
                    vs = 1;
                    exm.wiperFlag = true;
                    sds.cleanSpeed = 0.25f;
                    sds.paintHit_Tutorial();
                }
                if(vs == 1){
                    if(sds.wiperUp == 1 && !wiperTut){
                        if(time <= 1.5f){
                            time += Time.deltaTime;
                            GoodWip.SetActive(true);
                            WiperText.text = "";
                        }
                        if(time > 1.5f){
                            GoodWip.SetActive(false);
                            TutAvaWip.clip = wiperL;
                            wiperTut = true;
                            WiperText.text = "次に，左肩を前にひねって\nワイパーを下げよう！！";
                        }   
                    }
                    if(sds.wiperUp == -1 && wiperTut){
                        time += Time.deltaTime;
                        WiperText.text = "";
                        GoodWip.SetActive(true);
                        if(time >= 4f){
                            GoodWip.SetActive(false);
                            WiperText.text = "";
                            vs = 2;
                        } 
                    }
                }
                if(vs == 2){
                    col = true;
                    exm.wiperFlag = false;
                    FrontRaw.SetActive(false);
                    CnsShader.SetActive(true);
                    ChangeVideoNum();
                    time = 0;
                    sds.cleanSpeed = 0.25f;
                }
            }
        }


        // アイテムギミック（樽）
        if(videoNum == 4){
            wiper.SetActive(false);
            item.SetActive(true);
            TutText.text = "アイテムギミック";
            if(exp){
                ExpItem.SetActive(true);
                time += Time.deltaTime;
                if(time > 2){
                    exp = false;
                    ExpItem.SetActive(false);
                    TutItem.SetActive(true);
                    time = 0;
                }
            }
            if(!exp){
                if(vs == 3){
                    time += Time.deltaTime;
                }
                if(time > 1 && itemFlag){
                    Debug.Log("FSFS");
                    GobText.SetActive(true);
                    DashText.SetActive(false);
                    vs = 0;
                    itemFlag = false;
                }
                if(vs == 0){
                    Debug.Log("End1");
                    VideoStart(GobVideo);
                    DashVideo.Stop();   
                }
                if(vs == 1){
                    Debug.Log("End2");
                    VideoStop(GobVideo,1);
                }
                if(vs == 2){
                    Debug.Log("End3");
                    ChangeVideoNum();
                    vs = 0;
                }  
            }
            
        }

        // アイテムギミック（ダッシュアイテム）
        if(videoNum == 5){
            Debug.Log("End4");
            GobText.SetActive(false);
            DashText.SetActive(true);
            if(vs == 0){
                VideoStart(DashVideo);
                GobVideo.Stop();
            }
            if(vs == 1){
                VideoStop(DashVideo,1);
            }
            if(vs == 2){
                item.SetActive(false);
                // TutCan.SetActive(false);
                ChangeVideoNum();
            }
        }

        // ゲームスタート待機画面
        if(videoNum == 6){
            gameStart.SetActive(true);
            exm.wiperFlag = true;
            exm.hundleflag = true;
            if(exm.jumpF){
                TutCan.SetActive(false);
                ChangeVideoNum();
            }
        }

        // カウントダウン
        if(videoNum == 7){
            time += Time.deltaTime;
            if(time <= 1.0){
                countDown.text = "5";
            }
            if(time > 1.0){
                countDown.text = "4";
            }
            if(time > 2.0){
                countDown.text = "3";
            }
            if(time > 3.0){
                countDown.text = "2";
            }
            if(time > 4.0){
                countDown.text = "1";
            }
            if(time > 5.0){
                countDown.text = "Start";
            }
            if(time > 6.0){
                exm.wiperFlag = true;
                WipCam.SetActive(false);
                rb.isKinematic = false;
                rsp.respown = rsp.startpoint;
                this.GetComponent<ScoreRecorder>().raceBegin = true;
                bgm.GetComponent<AudioSource>().volume = 1.0f;
                ChangeVideoNum();
            }
        }
    }


    // 状態初期化
    public void Initialize(){
        time = 0f;
        vs = 3;
        itemFlag = true;
        exp = true;
        count = 0;
        calNum = 0;
        videoNum = 0;
        LoadNum = 0;
        wiperTut = false;
        col = true;
        TutCan.SetActive(true);
        calibration.SetActive(false);
        hundle.SetActive(false);
        jump.SetActive(false);
        wiper.SetActive(false);
        gameStart.SetActive(false);
        item.SetActive(false);
        HundleCam.SetActive(false);
        TopCam.SetActive(false);
        CnsShader.SetActive(false);
        WipCam.SetActive(true);
    }

    // チュートリアルシーン切り替え
    void ChangeVideoNum(){
        time = 0f;
        vs = 3;
        exp = true;
        count = 0;
        videoNum++;
    }

    // ビデオ再生
    void VideoStart(VideoPlayer vp){
        vp.loopPointReached += LoopPointReached;
        vp.isLooping = true;
        vp.Play();
        vs = 1;
    }

    // ビデオループ回数カウント
    public void LoopPointReached(VideoPlayer vp)
    {
        Debug.Log("End");
        count ++;  
    }

    // ビデオ停止　cntでビデオのループ回数指定
    void VideoStop(VideoPlayer vp, int cnt){
        if(count >= cnt){
          vp.loopPointReached -= LoopPointReached;
          vp.Stop();
          count = 0;
          vs = 2;
        } 
    }


    void OnTriggerEnter(Collider other){
        // ハンドル操作チュートリアル終了
        if(other.gameObject.name == "Tutrial_hundle"){
            Debug.Log(other.gameObject.name);
            if(!exp && col){
                col = false;
                count = 0;
                vs = 2;
            }  
        }

        // ジャンプチュートリアル開始
        if(other.gameObject.name == "Tutrial_Jump_Start"){
            if(exp && col){
                col = false;
                rb.isKinematic = true;
                rsp.RespownPos(rsp.respown); 
            }
            Debug.Log("Check");
        }

        // ジャンプタイミング提示のため一時停止
        if(other.gameObject.name == "Tutrial_Jump_Timing"){
            if(!exp && col){
                TutAvaJum.Play();
                col = false;
                Debug.Log("Check2");
                JumpTiming.text = "両肩を同時に上げよう！";
                rb.isKinematic = true; 
            } 
        }

        // ジャンプチュートリアル終了
        if(other.gameObject.name == "Tutrial_Jump_End"){
            if(!exp){
                col = false;
                count = 0;
                vs = 2;
                rb.isKinematic = true; 
                rsp.RespownPos(rsp.startpoint);
                
            }
        }
    }
}
