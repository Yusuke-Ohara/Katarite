using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    private bool judgeresult;//キャリブレーション結果取得
    public GameObject Calli;
    Calibration judge;

    private bool judgemotion;//モーション判定取得
    public GameObject receive;
    Example motion;
    Respown res;

    public GameObject SMG;
    JoyDis joydis;

    public GameObject bgmSource;


    public GameObject Katarite_Logo;
    public GameObject Text_Logo;
    public GameObject RelaxBar;
    public GameObject Text_Callibration;
    public GameObject RawImage_Callibration;
    public GameObject Text_Handle;
    public GameObject RawImage_Handle;
    public GameObject Text_Jump;
    public GameObject RawImage_Jump;
    public GameObject Text_WiperUP;
    public GameObject RawImage_WiperUP;
    public GameObject Text_WiperDOWN;
    public GameObject RawImage_WiperDOWN;
    public GameObject Text_GuardR;
    public GameObject RawImage_GuardR;
    public GameObject Text_GuardL;
    public GameObject RawImage_GuardL;
    public GameObject Good;
    public GameObject Ready;
    public GameObject Countdown;
    public GameObject Background;

    //SE関連
    public AudioClip goodSE;
    public AudioSource SESource;
    private bool GoodSEflag = false;

    //タイマー関連
    private float handle_timer = 0;
    private float good_timer = 0;
    private float countdown_timer = 0;
    public int uiflag = 1; //0:スタート画面、1:安静指示画面、2：キャリブレーション画面、3:ハンドル確認画面
        //4：ジャンプ、5：ワイパーアップ、6:ワイパーダウン、7：ガードR、8:ガードL、9:ready&countdown
    private bool good_flag = false;
    private bool pose;

    void Start()
    {   
        //Calli.GetComponent<Calibration>().enabled = false;
        judge = Calli.GetComponent<Calibration>();
        motion = receive.GetComponent<Example>();
        res = receive.GetComponent<Respown>();
        receive.GetComponent<Rigidbody>().isKinematic = true;
        joydis = receive.GetComponent<JoyDis>();
    }

    // Update is called once per frame
    void Update()
    {   
        Debug.Log("JoyConne" + joydis.joyConnect);
        if(joydis.joyConnect){
            if(Input.GetKeyDown(KeyCode.Q)){
                if(pose){
                    pose = false;
                }else{
                    pose = true;
                }
            }
            if(Input.GetKeyDown(KeyCode.R)){
                Restart();
            }
            Debug.Log("qqq" + uiflag);
            // if (uiflag == 0 && Input.GetKeyDown(KeyCode.A/*Joystick1Button0*/)) {// スタート画面表示
            //     Katarite_Logo.SetActive(true);
            //     Text_Logo.SetActive(true);
            //     Calli.GetComponent<Calibration>().enabled = true;
            //     RelaxBar.SetActive(true);
            //     uiflag = 1;
            // }
            if(uiflag == 1){
                judgeresult = judge.Relax;
                Debug.Log("ppp" + judgeresult);
            }
            
            if (uiflag == 1 && judgeresult == true) {// キャリブレーション画面表示
                Katarite_Logo.SetActive(false);
                Text_Logo.SetActive(false);
                RelaxBar.SetActive(false);
                RawImage_Callibration.SetActive(true);
                Text_Callibration.SetActive(true);
                uiflag = 2;
            }

            if(uiflag == 2){
                judgeresult = judge.calend;
                Debug.Log("ppp" + judgeresult);
            }
            if(uiflag == 2 && judgeresult == true){// ハンドル画面表示
                motion.Load();
                RawImage_Callibration.SetActive(false);
                Text_Callibration.SetActive(false);
                RawImage_Handle.SetActive(true);
                Text_Handle.SetActive(true);
                motion.hundleflag = true;
                uiflag = 3;
            }
            if(uiflag == 3){//ハンドル表示時間タイマー
                if(pose){
                    handle_timer = 5.0f;
                }else if(!pose){
                    handle_timer += Time.deltaTime;
                }
            }
            if(uiflag == 3 && handle_timer > 10.0 || uiflag == 3 && Input.GetKeyDown(KeyCode.P)){//ジャンプ表示
                RawImage_Handle.SetActive(false);
                Text_Handle.SetActive(false);
                RawImage_Jump.SetActive(true);
                Text_Jump.SetActive(true);
                uiflag = 4;
            }
            if(uiflag == 4){
                judgemotion = motion.GetJumpFlag();
            }
            if(uiflag == 4 && judgemotion == true || uiflag == 4 && Input.GetKeyDown(KeyCode.P)){
                Good.SetActive(true);
                if(GoodSEflag == false){
                    SESource.PlayOneShot(goodSE);
                    GoodSEflag = true;
                }
                good_flag = true;
            }

            if(uiflag == 4 && good_flag == true){
                good_timer += Time.deltaTime;
            }

            if(uiflag == 4 && good_flag == true && good_timer > 2.0){
                good_flag = false;
                good_timer = 0;
                RawImage_Jump.SetActive(false);
                Text_Jump.SetActive(false);
                Good.SetActive(false);
                uiflag = 9;
                Ready.SetActive(true);
                Countdown.SetActive(true);
                // RawImage_WiperUP.SetActive(true);
                // Text_WiperUP.SetActive(true);
                //uiflag = 5;

            }

            if(uiflag == 9){
                countdown_timer += Time.deltaTime;
            }

            if(uiflag == 9 && countdown_timer > 1.0 && countdown_timer <= 2.0){
                Countdown.GetComponent<UnityEngine.UI.Text>().text = "4";
            }
            else if(uiflag == 9 && countdown_timer > 2.0 && countdown_timer <= 3.0){
                Countdown.GetComponent<UnityEngine.UI.Text>().text = "3";
            }
            else if(uiflag == 9 && countdown_timer > 3.0 && countdown_timer <= 4.0){
                Countdown.GetComponent<UnityEngine.UI.Text>().text = "2";
            }
            else if(uiflag == 9 && countdown_timer > 4.0 && countdown_timer <= 5.0){
                Countdown.GetComponent<UnityEngine.UI.Text>().text = "1";
            }
            else if(uiflag == 9 && countdown_timer > 5.0 && countdown_timer <= 5.5){
                Countdown.GetComponent<UnityEngine.UI.Text>().text = "Start!";
            }
            else if(uiflag == 9 && countdown_timer > 5.5){
                receive.GetComponent<Rigidbody>().isKinematic = false;
                bgmSource.GetComponent<AudioSource>().volume = 1.0f;
                receive.GetComponent<ScoreRecorder>().raceBegin = true;
                Ready.SetActive(false);
                Countdown.SetActive(false);
                Background.SetActive(false);
                uiflag = 10;
            }
        }




        /*ワイパー部分実装中
        // if(uiflag == 5){
        //     judgemotion = motion.GetJumpFlag();
        // }
        // if(uiflag == 4 && judgemotion == true || uiflag == 4 && Input.GetKeyDown(KeyCode.P)){
        //     Good.SetActive(true);
        //     good_flag = true;
        // }

        // if(uiflag == 4 && good_flag == true){
        //     good_timer += Time.deltaTime;
        // }

        // if(uiflag == 4 && good_flag == true && good_timer > 2.0){
        //     good_flag = false;
        //     RawImage_Jump.SetActive(false);
        //     Text_Jump.SetActive(false);
        //     Good.SetActive(false);
        //     RawImage_WiperUP.SetActive(true);
        //     Text_WiperUP.SetActive(true);
        //     uiflag = 5;
        // }
        ワイパー部分実装中*/


    }

    private void Restart(){
        res.nowGame = false;
        res.RespownPos(res.startpoint);
        receive.GetComponent<Rigidbody>().isKinematic = true;
        uiflag = 1;
        Katarite_Logo.SetActive(true);
        Text_Logo.SetActive(true);
        RelaxBar.SetActive(true);
        RawImage_Callibration.SetActive(false);
        Text_Callibration.SetActive(false);
        RawImage_Handle.SetActive(false);
        Text_Handle.SetActive(false);
        handle_timer = 0;
        RawImage_Jump.SetActive(false);
        Text_Jump.SetActive(false);
        Good.SetActive(false);
        good_flag = false;
        good_timer = 0;
        Ready.SetActive(false);
        Countdown.SetActive(false);
        countdown_timer = 0;
        Countdown.GetComponent<UnityEngine.UI.Text>().text = "5";
        Text_WiperUP.SetActive(false);
        RawImage_WiperUP.SetActive(false);;
        Text_WiperDOWN.SetActive(false);;
        RawImage_WiperDOWN.SetActive(false);;
        Text_GuardR.SetActive(false);;
        RawImage_GuardR.SetActive(false);;
        Text_GuardL.SetActive(false);;
        RawImage_GuardL.SetActive(false);;
        Background.SetActive(true);
        motion.hundleflag = false;
        bgmSource.GetComponent<AudioSource>().volume = 0.2f;
        GoodSEflag = false;
    }
}