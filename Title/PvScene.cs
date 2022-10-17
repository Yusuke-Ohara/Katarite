using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

// PVシーン（一定時間入力がない場合にPV再生）
public class PvScene : MonoBehaviour
{
    public VideoPlayer pv;
    void Start()
    {
        pv.Play();
    }

    void Update()
    {
        // なにか入力があった場合にタイトル画面へ遷移
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.LeftControl)){
            FadeManager.Instance.LoadScene("StartScene", 0.3f);
        }
    }
}
