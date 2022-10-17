using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistViewer : MonoBehaviour
{
    //アシストの有無を決める変数。慣れててうざったければfalseにすればアシスト画面が出なくなる
    public bool assistFlg;

    //アシスト画面
    public GameObject wiperAssist;
    public GameObject jumpAssist;
    public GameObject guardAssist; 
    //アシスト画面が出ているか管理する変数
    private bool wiperFlg;
    private bool jumpFlg;
    private bool guardFlg; 
    private bool wiperAssistIsRight;
    private bool guardAssistIsRight;

    // Start is called before the first frame update
    void Start()
    {
        wiperAssist.SetActive(false);
        jumpAssist.SetActive(false);
        guardAssist.SetActive(false);

        wiperFlg = false;
        jumpFlg = false;
        guardFlg = false;

        wiperAssistIsRight = false;
        guardAssistIsRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //アシスト画面操作のための関数
    public void UseAssistMovie(string action, bool toggle){
        if(assistFlg){
            if(action == "wiper"){
                wiperAssist.SetActive(toggle);
                wiperFlg = toggle;
            }else if(action == "jump"){
                jumpAssist.SetActive(toggle);
                jumpFlg = toggle;
            }else if(action == "guard"){
                guardAssist.SetActive(toggle);
                guardFlg = toggle;
            }
        }
    }

    //向き変更
    public void ChangeAssistDir(string action, bool isRight){
        if(action == "wiper"){
            if(wiperFlg){
                if(isRight ^ wiperAssistIsRight){
                    RectTransform rt = wiperAssist.GetComponent<RectTransform>();
                    rt.localScale = new Vector3(-rt.localScale.x, rt.localScale.y, rt.localScale.z);
                    wiperAssistIsRight = !wiperAssistIsRight;
                }
            }
        }else if(action == "guard"){
            if(guardFlg){
                if(isRight ^ guardAssistIsRight){
                    RectTransform rt = guardAssist.GetComponent<RectTransform>();
                    rt.localScale = new Vector3(-rt.localScale.x, rt.localScale.y, rt.localScale.z);
                    guardAssistIsRight = !guardAssistIsRight;
                }
            }
        }
    }
}
