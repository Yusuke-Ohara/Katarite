using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class BootPython : MonoBehaviour
{
    public string exePath;
    Process process;
    
    void Awake(){
        //exePath = "C:/Users/mictl/Documents/Unity/Projects/KatariteProject/Assets/Resources/python/dist/shoulder4unity.exe";
        exePath = Application.dataPath + "/shoulder4unity.exe";
        ProcessStartInfo processStartInfo = new ProcessStartInfo(){
            FileName = exePath,
            UseShellExecute = false,//シェルを使うかどうか
            CreateNoWindow = false, //ウィンドウを開くかどうか
            RedirectStandardOutput = true, //テキスト出力をStandardOutputストリームに書き込むかどうか
            //Arguments = "ARGUMENT", //実行するスクリプト 引数(複数可)
        };

        //外部プロセスの開始
        process = Process.Start(processStartInfo);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }
    

    void OnApplicationQuit(){
        var processes = Process.GetProcessesByName("shoulder4unity");
        foreach(var killProcess in processes){
            killProcess.Kill();
        }
        
    }
}
