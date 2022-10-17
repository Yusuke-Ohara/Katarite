using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;

public class PythonCollabTest : MonoBehaviour
{
    //python実行ファイルアドレス指定
    private string pyExePath = @"python.exe";

    //Pythonソースコードファイルアドレス指定
    private string pyCodePath = @"C:\Users\mictle\Documents\PythonScripts\katariteEx\unityTest.py";

    // Start is called before the first frame update
    void Start()
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo()
        {
            FileName = pyExePath,
            UseShellExecute = false, //シェルを開くかどうか
            CreateNoWindow = true, //新しいウインドウを開くかどうか
            RedirectStandardOutput = true, //テキスト出力をStandardOutputストリームに書き込むかどうか
            Arguments = pyCodePath + " " + "Hello, Python.", //実行スクリプトの引数
        };

        //外部プロセス開始
        Process process = Process.Start(processStartInfo);

        //ストリームから出力を得る
        StreamReader streamReader = process.StandardOutput;
        string str = streamReader.ReadLine();

        //外部プロセス終了
        process.WaitForExit();
        process.Close();

        print(str);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
