using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


// タイトル画面制御（ユーザ名入力～モード選択）
public class SelectMode : MonoBehaviour
{
    private int scene; //現在のシーン（-1：ユーザー名確認，1：初期画面＋ユーザー名入力待機，2：種目選択，3：難易度orモード選択）
    
    JoyconD joyD;

    // 効果音
    public AudioClip EnterSound;
    public AudioClip LastEnterSound;
    public AudioClip AnyButtonSound;
    public AudioClip ErrorSound;

    AudioSource audioSource;

    private float timer;

    //KeyGuid（Enterキーで決定，BackSpaceで戻る）
    public GameObject Enter;
    public GameObject backSpace;
    public Text backSpaceText;
    
    //Scene1　初期画面＋ユーザー名入力待機
    public GameObject scene1;
    public InputField name;
    private int userNumber;
    bool test;
    public GameObject discon;
    public GameObject namecheck;
    public Text username;
    public GameObject movetext;
    ImgMove imgmove;
    public Text kettei;
    public Text cancel;
    private int enter;
    private bool ok;
    private float poseTime;
    private bool poseFlag;
    public float changeTime;
    private int skiCount;
    public Text skiPlayCountText;

    //Scene2　種目選択
    public GameObject scene2;
    public Image Drive;
    public RawImage Drivemv;
    public Image Ski;
    public RawImage Skimv;    
    private bool shoulderDrive;
    
    
    //Scene3　難易度orモード選択
    public GameObject scene3;
    private int level;
    public GameObject Drive_level;
    public Image Drive_easy;
    public Image Drive_normal;
    public Image Drive_hard;
    public GameObject Ski_mode;
    public Image Ski_slalom;
    public Image Ski_aerial;
    public Image Ski_free;


    void Start()
    {
        // SoulderSkiを遊んだ回数（1人2回）
        if(PlayerPrefs.GetInt("skiPlayCount",0) != 1){
            scene = 1;
            shoulderDrive = true;
            skiCount = 2;
            enter = 0;
        }else{
            scene = 3;
            shoulderDrive = false;
            skiCount = 1;
            enter = 1;
        }
        
        level = 1;
        ok = true;
        poseTime = 0;
        name.ActivateInputField();
        // name.onEndEdit.AddListener(
        //     delegate{
        //         name.ActivateInputField();
        //     }
        // );
        userNumber =  PlayerPrefs.GetInt("guestNum",1);
        name.text  = "Guest" + userNumber;
        imgmove = movetext.GetComponent<ImgMove>();
        joyD = GetComponent<JoyconD>();
        audioSource = GetComponent<AudioSource>();
        backSpaceText.text = "BackSpace\nで戻る";
        scene1.SetActive(false);
        scene2.SetActive(false);
        scene3.SetActive(false);
        timer = 0;
    }

    void Update()
    {
        Debug.Log("en" + timer);

        // 効果音の処理
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Backspace)){
            audioSource.PlayOneShot(AnyButtonSound);
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            if(joyD.joyConnect){
                if(scene == 3){
                    audioSource.PlayOneShot(LastEnterSound);
                }else{
                    audioSource.PlayOneShot(EnterSound);
                }
                
            }else{
                audioSource.PlayOneShot(ErrorSound);
            }
        }

        if(scene == -1){
            Enter.SetActive(false);
        }else{
            Enter.SetActive(true);
        }


        // 初期画面＋ユーザー名入力待機
        if(scene == 1){
            backSpace.SetActive(false);
            scene1.SetActive(true);
            scene2.SetActive(false);
            if(Input.anyKeyDown){
                poseTime = 0;
                poseFlag = false;
            }else{
                poseTime += Time.deltaTime;
            }
            if(poseTime >= changeTime && !poseFlag){
                poseFlag = true;
            }
            if(poseFlag){
                FadeManager.Instance.LoadScene("PV", 0.3f);
            }else{
                if(Input.GetKeyDown(KeyCode.RightAlt)){
                    test = true;
                    name.text  = "TEST";
                }
                if(Input.GetKeyDown(KeyCode.LeftAlt)){
                    name.text  = "Guest" + userNumber;
                    test = false;
                }
                if(Input.GetKeyDown(KeyCode.Return)){
                    // Joy-Conの接続状況提示
                    if(joyD.joyConnect){
                        imgmove.movespeed = 0;
                        namecheck.SetActive(true);
                        username.text = name.text;
                        scene = -1;  
                    }else{
                        discon.SetActive(true);
                        if(Input.GetKeyDown(KeyCode.Return)){
                            enter++;
                            if(enter >= 2){
                                Debug.Log("RS");
                                FadeManager.Instance.LoadScene("StartScene", 0.3f);
                            }
                        }
                    }  
                }
            }
            
        }

        // ユーザー名確認
        if(scene == -1){
            backSpace.SetActive(false);
            if(Input.GetKeyDown(KeyCode.Return)){
                enter++;
                if(enter >= 2){
                    Debug.Log("ok true");
                    scene1.SetActive(false);
                    namecheck.SetActive(false);
                    scene2.SetActive(true);
                    scene = 2;
                    enter = 0;
                    PlayerPrefs.SetString("UserName",name.text);
                    if(!test){
                        PlayerPrefs.SetInt("guestNum", userNumber + 1);
                    }
                    PlayerPrefs.Save(); 
                    Debug.Log(PlayerPrefs.GetString("UserName","0")); 
                }     
            }

            if(Input.GetKeyDown(KeyCode.Backspace)){
                Debug.Log("ok false");
                imgmove.movespeed = 70;
                namecheck.SetActive(false);
                scene = 1;
                ok = true;
                enter = 0;
                name.ActivateInputField(); 
            }
        }

        // 種目選択
        if(scene == 2){
            backSpace.SetActive(true);
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                shoulderDrive = false;
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                shoulderDrive = true;
            }

            if(shoulderDrive){
                Drive.color = new Color(81f/255f, 255f/255f, 239f/255f);
                Drivemv.color = new Color(255f/255f, 255f/255f, 255f/255f);
                Ski.color = new Color(50f/255f, 50f/255f, 50f/255f); 
                Skimv.color = new Color(100f/255f, 100f/255f, 100f/255f);
            }else{
                Drive.color = new Color(40f/255f, 150f/255f, 150f/255f);
                Drivemv.color = new Color(100f/255f, 100f/255f, 100f/255f);
                Ski.color = new Color(255f/255f, 255f/255f, 255f/255f); 
                Skimv.color = new Color(255f/255f, 255f/255f, 255f/255f);
            }
            if(Input.GetKeyDown(KeyCode.Return)){
                enter++;
                if(enter >= 2){
                    scene = 3;
                    enter = 0;
                    scene2.SetActive(false);
                } 
            }
            if(Input.GetKeyDown(KeyCode.Backspace)){
                scene = 1;
                ok = true;
                shoulderDrive = true;
                enter = 0;
                scene2.SetActive(false);
                scene1.SetActive(true);
                name.ActivateInputField(); 
            }
        }


        // 難易度（ShoulderDrive）orモード選択（ShoulderSki）
        if(scene == 3){
            scene3.SetActive(true);
            if(shoulderDrive){
                Drive_level.SetActive(true);
                if(Input.GetKeyDown(KeyCode.RightArrow)){
                    if(level < 2){
                        level++;
                    } 
                }
                if(Input.GetKeyDown(KeyCode.LeftArrow)){
                    if(level > 0){
                        level--;
                    } 
                }
            }else{
                Ski_mode.SetActive(true);
                if(Input.GetKeyDown(KeyCode.RightArrow)){
                    if(level < 1){
                        level++;
                    } 
                }
                if(Input.GetKeyDown(KeyCode.LeftArrow)){
                    if(level > 0){
                        level--;
                    } 
                }
            } 

            if(!shoulderDrive && skiCount == 1){
                backSpaceText.text = "BackSpace\nで終了";
            }

            // if(Input.GetKeyDown(KeyCode.RightArrow)){
            //     if(level < 2){
            //         level++;
            //     } 
            // }
            // if(Input.GetKeyDown(KeyCode.LeftArrow)){
            //     if(level > 0){
            //         level--;
            //     } 
            // }

            if(!shoulderDrive){
                skiPlayCountText.text = "あと" + skiCount + "回遊べるよ";
            }

            if(level == 0){
                if(shoulderDrive){
                    Drive_easy.color = new Color(100f/255f, 255f/255f, 255f/255f);
                    Drive_normal.color = new Color(100f/255f, 100f/255f, 50f/255f);
                    Drive_hard.color = new Color(130f/255f, 60f/255f, 60f/255f);
                }else{
                    Ski_slalom.color = new Color(100f/255f, 255f/255f, 255f/255f);
                    Ski_aerial.color = new Color(100f/255f, 100f/255f, 50f/255f);
                    Ski_free.color = new Color(130f/255f, 60f/255f, 60f/255f);
                }
                
            }
            if(level == 1){
                if(shoulderDrive){
                    Drive_easy.color = new Color(50f/255f, 100f/255f, 100f/255f);
                    Drive_normal.color = new Color(255f/255f, 255f/255f, 100f/255f);
                    Drive_hard.color = new Color(130f/255f, 60f/255f, 60f/255f);
                }else{
                    Ski_slalom.color = new Color(50f/255f, 100f/255f, 100f/255f);
                    Ski_aerial.color = new Color(255f/255f, 255f/255f, 100f/255f);
                    Ski_free.color = new Color(130f/255f, 60f/255f, 60f/255f);
                }
                
            }
            if(level == 2){
                if(shoulderDrive){
                    Drive_easy.color = new Color(50f/255f, 100f/255f, 100f/255f);
                    Drive_normal.color = new Color(100f/255f, 100f/255f, 50f/255f);
                    Drive_hard.color = new Color(255f/255f, 130f/255f, 130f/255f);
                }
                // else{
                //     Ski_slalom.color = new Color(50f/255f, 100f/255f, 100f/255f);
                //     Ski_aerial.color = new Color(100f/255f, 100f/255f, 50f/255f);
                //     Ski_free.color = new Color(255f/255f, 130f/255f, 130f/255f);
                // }
            }
            if(Input.GetKeyDown(KeyCode.Return)){
                enter++;
                if(enter >= 2){
                    timer += Time.deltaTime;
                    //if(timer >= 4.0f){
                        if(shoulderDrive){
                            PlayerPrefs.SetInt("Drive_level", level);
                            PlayerPrefs.Save();
                            skiCount = 2;
                            FadeManager.Instance.LoadScene("GameScene", 0.8f);
                        }else{
                            if(level == 0){
                                PlayerPrefs.SetInt("Drive_level", 3);
                                FadeManager.Instance.LoadScene("Ski_Slalom", 0.8f);
                            }else if(level == 1){
                                PlayerPrefs.SetInt("Drive_level", 4);
                                FadeManager.Instance.LoadScene("Ski_Airial", 0.8f);
                            }
                            // else{
                            //     FadeManager.Instance.LoadScene("Ski_Free", 0.8f);
                            // }
                            if(!test){
                                PlayerPrefs.SetInt("skiPlayCount", skiCount - 1);
                                PlayerPrefs.Save();
                            }
                        }
                    //}
                } 
            }

            if(shoulderDrive || skiCount != 1 || test){
                if(Input.GetKeyDown(KeyCode.Backspace)){
                    scene = 2;
                    level = 1;
                    enter = 1;
                    timer = 0.0f;
                    scene3.SetActive(false);
                    Drive_level.SetActive(false);
                    Ski_mode.SetActive(false);
                    scene2.SetActive(true);
                }
            }else{
                if(Input.GetKeyDown(KeyCode.Backspace)){
                    timer = 0.0f;
                    PlayerPrefs.SetInt("skiPlayCount", 2);
                    PlayerPrefs.Save();
                    FadeManager.Instance.LoadScene("StartScene", 0.3f);
                }
            }
        }

    }
}
