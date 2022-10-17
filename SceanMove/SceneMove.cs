using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//シーン遷移　右Shiftでシーンリロード
public class SceneMove : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {

        // if(StartScene){
            if(Input.GetKeyDown(KeyCode.RightControl)){
                FadeManager.Instance.LoadScene("GameScene", 0.3f);
            }
        // }else{
            if(/*m_joyconR.GetButtonDown(Joycon.Button.DPAD_DOWN) || */Input.GetKeyDown(KeyCode.RightControl)){
                FadeManager.Instance.LoadScene("StartScene", 0.3f);
            }
            
        // }
        if(Input.GetKeyDown(KeyCode.RightShift)){
                FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 0.3f);
            }
    }
}
