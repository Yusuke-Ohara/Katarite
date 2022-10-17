using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public enum TutorialMode{
    calibRelax,
    calibMeasure,
    calibEnd,
    direction,
    speedUp,
    flags,
    jumpRump,
    jumpTiming,
    jumpHorizontal,
    jumpSpecial,
    jumpRule,
    gameReady,
    countDown,
    none
}

public enum SkiMode{
    slalom,
    airial,
    free
}

public class SkiTutorial : MonoBehaviour
{
    [SerializeField] private SkiMode skiMode;

    [SerializeField] private GameObject skiPlayer;
    [SerializeField] private Text tutorialText;
    [SerializeField] private GameObject upperPanel;
    [SerializeField] private GameObject skipText;
    [SerializeField] private Ski_gate_open skiGateLeft;
    [SerializeField] private Ski_gate_open skiGateRight;

   
    [SerializeField] private GameObject sceneManager;

    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject calibObj;
    [SerializeField] private GameObject directionObj;
    [SerializeField] private GameObject gimmickObj;
    [SerializeField] private GameObject gameReadyObj;
    [SerializeField] private AudioSource BGMSource;

    [SerializeField] private GameObject calibRelax;
    [SerializeField] private GameObject calibMeasure;
    [SerializeField] private GameObject calibEnd;

    //directionチュートリアル
    [SerializeField] private GameObject direction;
    [SerializeField] private VideoPlayer directionExpainVideoPlayer;

    //speedUpチュートリアル
    [SerializeField] private GameObject speedUp;
    [SerializeField] private VideoPlayer speedUpExpainVideoPlayer;

    //flagsチュートリアル
    [SerializeField] private GameObject flags;
    [SerializeField] private VideoPlayer flagExplainVideoPlayer;


    [SerializeField] private GameObject jumpRump;
    [SerializeField] private VideoPlayer jumpRumpVideoPlayer;

    [SerializeField] private GameObject jumpTiming;
    [SerializeField] private VideoPlayer jumpTimingVideoPlayer;

    [SerializeField] private GameObject jumpHorizontal;
    [SerializeField] private VideoPlayer jumpHorizontalVideoPlayer;

    [SerializeField] private GameObject jumpSpecial;
    [SerializeField] private VideoPlayer jumpSpecialVideoPlayer;

    [SerializeField] private GameObject jumpRule;



    [SerializeField] private GameObject gameReady;

    //カウントダウン
    [SerializeField] private GameObject countDown;
    [SerializeField] private Text countDownText;


    //キャリブレーションを使うか否か
    [SerializeField] private TutorialMode startMode;


    private Calibration calib;
    private Example example;
    private skiing_movement skMovement;
    private ScoreRecorde_Skiing scoreRecord;
    private TutorialMode tutorialMode = TutorialMode.calibRelax;
    private float timer;
    private float timerLimit;
    private int count = 0;

    private Stack<TutorialMode> prevModes = new Stack<TutorialMode>();

    [SerializeField] float calibrationTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        prevModes.Push(TutorialMode.direction);
        calib = tutorialCanvas.GetComponent<Calibration>();
        example = skiPlayer.GetComponent<Example>();
        scoreRecord = skiPlayer.GetComponent<ScoreRecorde_Skiing>();
        if(PlayerPrefs.GetInt("skiPlayCount", 2) == 0){
            startMode = TutorialMode.direction;
            calib.CalSkip();
        }
        StartMode(startMode);
        skMovement = skiPlayer.GetComponent<skiing_movement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(tutorialMode){
            case TutorialMode.calibRelax:
                ChangeModeByFlag(calib.relax, TutorialMode.calibRelax, TutorialMode.calibMeasure);
                break;

            case TutorialMode.calibMeasure:
                ChangeModeByFlag(calib.calend, TutorialMode.calibMeasure, TutorialMode.calibEnd);
                break;

            case TutorialMode.calibEnd:
                ChangeModeByFlag(TimerUpdate(), TutorialMode.calibEnd, TutorialMode.direction);
                break;

            case TutorialMode.direction:
                ChangeModeByFlag(VideoStop(directionExpainVideoPlayer, 1), TutorialMode.direction, TutorialMode.speedUp);
                break;

            case TutorialMode.speedUp:
                if(skiMode == SkiMode.slalom)ChangeModeByFlag(VideoStop(speedUpExpainVideoPlayer, 2), TutorialMode.speedUp, TutorialMode.flags);
                else if(skiMode == SkiMode.airial) ChangeModeByFlag(VideoStop(speedUpExpainVideoPlayer, 2), TutorialMode.speedUp, TutorialMode.jumpRump);
                break;

            case TutorialMode.flags:
                ChangeModeByFlag(TimerUpdate(), TutorialMode.flags, TutorialMode.gameReady);
                break;

            case TutorialMode.jumpRump:
                ChangeModeByFlag(VideoStop(jumpRumpVideoPlayer, 2), TutorialMode.jumpRump, TutorialMode.jumpTiming);
                break;

            case TutorialMode.jumpTiming:
                ChangeModeByFlag(VideoStop(jumpTimingVideoPlayer, 2), TutorialMode.jumpTiming, TutorialMode.jumpHorizontal);
                break;

            case TutorialMode.jumpHorizontal:
                ChangeModeByFlag(VideoStop(jumpHorizontalVideoPlayer, 2), TutorialMode.jumpHorizontal, TutorialMode.jumpSpecial);
                break;

            case TutorialMode.jumpSpecial:
                ChangeModeByFlag(VideoStop(jumpSpecialVideoPlayer, 3), TutorialMode.jumpSpecial, TutorialMode.jumpRule);
                break;

            case TutorialMode.jumpRule:
                ChangeModeByFlag(TimerUpdate(), TutorialMode.jumpRule, TutorialMode.gameReady);
                break;
            case TutorialMode.gameReady:
                tutorialText.text = "";
                ChangeModeByFlag(TimerUpdate() && (example.jumpF || Input.GetKeyDown(KeyCode.X)), TutorialMode.gameReady, TutorialMode.countDown);
                break;

            case TutorialMode.countDown:
                if(TimerUpdate()) EndMode(TutorialMode.countDown);
                else if(6.0f - timer < 1.0f) countDownText.text = "Start!!";
                else countDownText.text = "" + Math.Floor(6.0f - timer);
                break;
        }

        if(Input.GetKeyDown(KeyCode.Backspace)){
            EndMode(tutorialMode);
            if(prevModes.Count<=1) StartMode(prevModes.Peek());
            else StartMode(prevModes.Pop());
        }
    }

    public void StartMode(TutorialMode mode){
        switch(mode){
            case TutorialMode.calibRelax:
                tutorialText.text = "準備運動(肩の動き計測中)";
                calibObj.SetActive(true);
                calibRelax.SetActive(true);
                calibMeasure.SetActive(false);
                calibEnd.SetActive(false);
                upperPanel.SetActive(true);
                break;

            case TutorialMode.calibMeasure:
                calibMeasure.SetActive(true);
                break;

            case TutorialMode.calibEnd:
                calibEnd.SetActive(true);
                SetTimer(calibrationTime);
                break;

            case TutorialMode.direction:
                tutorialText.text = "操作方法";
                skipText.SetActive(true);
                direction.SetActive(true);
                VideoStart(directionExpainVideoPlayer);
                break;

            case TutorialMode.speedUp:
                tutorialText.text = "操作方法";
                speedUp.SetActive(true);
                VideoStart(speedUpExpainVideoPlayer);
                break;

            case TutorialMode.flags:
                tutorialText.text = "ルール";
                flags.SetActive(true);
                SetTimer(3.0f);
                VideoStart(flagExplainVideoPlayer);
                break;

            case TutorialMode.jumpRump:
                tutorialText.text = "ジャンプ台";
                jumpRump.SetActive(true);
                VideoStart(jumpRumpVideoPlayer);
                break;

            case TutorialMode.jumpTiming:
                jumpTiming.SetActive(true);
                VideoStart(jumpTimingVideoPlayer);
                break;

            case TutorialMode.jumpHorizontal:
                tutorialText.text = "ジャンプアクション";
                jumpHorizontal.SetActive(true);
                VideoStart(jumpHorizontalVideoPlayer);
                break;

            case TutorialMode.jumpSpecial:
                jumpSpecial.SetActive(true);
                VideoStart(jumpSpecialVideoPlayer);
                break;

            case TutorialMode.jumpRule:
                jumpRule.SetActive(true);
                SetTimer(5.0f);
                break;

            case TutorialMode.gameReady:
                tutorialText.text = "";
                gameReady.SetActive(true);
                SetTimer(0.5f);
                break;

            case TutorialMode.countDown:
                upperPanel.SetActive(false);
                countDown.SetActive(true);
                SetTimer(6.0f);
                break;
        }
        tutorialMode = mode;
    }

    public void EndMode(TutorialMode mode){
        switch(mode){
            case TutorialMode.calibRelax:
                calibRelax.SetActive(false);
                break;

            case TutorialMode.calibMeasure:
                calibMeasure.SetActive(false);
                break;

            case TutorialMode.calibEnd:
                example.Load();
                calibEnd.SetActive(false);
                calibObj.SetActive(false);
                break;

            case TutorialMode.direction:
                direction.SetActive(false);
                break;

            case TutorialMode.speedUp:
                speedUp.SetActive(false);
                break;
            
            case TutorialMode.flags:
                flags.SetActive(false);
                break;

            case TutorialMode.jumpRump:
                jumpRump.SetActive(false);
                break;

            case TutorialMode.jumpTiming:
                jumpTiming.SetActive(false);
                break;

            case TutorialMode.jumpHorizontal:
                jumpHorizontal.SetActive(false);
                break;

            case TutorialMode.jumpSpecial:
                jumpSpecial.SetActive(false);
                break;

            case TutorialMode.jumpRule:
                jumpRule.SetActive(false);
                break;

            case TutorialMode.gameReady:
                gameReady.SetActive(false);
                break;

            case TutorialMode.countDown:
                countDown.SetActive(false);
                skMovement.Ski_Start();
                scoreRecord.RaceStart();
                skiGateLeft.GateOpen();
                skiGateRight.GateOpen();
                BGMSource.volume = 1.0f;
                tutorialMode = TutorialMode.none;
                break;

            
        }
    }

    private void SetTimer(float lim){
        timer = 0;
        timerLimit = lim;
    }

    private bool TimerUpdate(){
        if(timer>=timerLimit)return true;
        timer += Time.deltaTime;
        return (timer>=timerLimit);
    }

    private void ChangeModeByFlag(bool flag, TutorialMode endMode, TutorialMode startMode){
        if(flag || Input.GetKeyDown(KeyCode.Return)){
            EndMode(endMode);
            StartMode(startMode);
            if((int)endMode <= (int)TutorialMode.calibEnd) prevModes.Push(TutorialMode.direction);
            else prevModes.Push(endMode);
            
        }
    }

    void VideoStart(VideoPlayer vp){
        vp.loopPointReached += LoopPointReached;
        vp.isLooping = true;
        count=0;
        vp.Play();
    }

    public void LoopPointReached(VideoPlayer vp)
    {
        Debug.Log("End");
        count ++;  
    }

    bool VideoStop(VideoPlayer vp, int cnt){
        if(count >= cnt){
        vp.loopPointReached -= LoopPointReached;
          vp.Stop();
          count = 0;
          return true;
        }
        return false;
    }
}
