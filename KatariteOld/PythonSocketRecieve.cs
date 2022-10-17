using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class PythonSocketRecieve : MonoBehaviour
{
    // Start is called before the first frame update
    static UdpClient udpClient;
    IPEndPoint remoteEP = null;
    
    public float wiperCoolTime;
    public float guardCoolTime;

    public float jumpCoolTime;

    public float guardUpTime;

    [SerializeField]private float wiperLNowTime, wiperRNowTime, guardLNowTime, guardRNowTime, jumpNowTime;

    private bool wiperUp, guardLflg, guardRflg;

    private ShoulderDriveSimple shoulderDriveSimple;

    public int logLength;

    private Queue<float> log_rx, log_lx;
    private int rxCount, lxCount;

    public float mawasuXRate;

    public int mawasuXCount;

    private string rcvText;

    private int judgeNumber;

    private JoyconData joyconData;
    void Start()
    {
        int LOCA_LPORT = 50007;
        //Pythonなし時CO
        //udpClient = new UdpClient(LOCA_LPORT);
        //udpClient.BeginReceive(ReceiveCallback, udpClient);

        log_rx = new Queue<float>();
        log_lx = new Queue<float>();
        wiperLNowTime = 0;
        wiperRNowTime = 0;
        guardLNowTime = 0;
        guardRNowTime = 0;
        jumpNowTime = 0;
        shoulderDriveSimple = this.GetComponent<ShoulderDriveSimple>();
        wiperUp = false;
        guardLflg = guardRflg = false;

        rxCount = 0;
        lxCount = 0;

        for(int i=0; i < logLength; i++)
        {
            log_rx.Enqueue(0);
            log_lx.Enqueue(0);
        }

        joyconData = GetComponent<JoyconData>();
    }

    // Update is called once per frame

    void Update(){
        
        //回す判定用xlog作成
        if (log_rx.Dequeue() >= mawasuXRate) rxCount--;
        if (log_lx.Dequeue() >= mawasuXRate) lxCount--;
        float xr = (float)joyconData.readXR();
        float xl = (float)joyconData.readXL();
        log_rx.Enqueue(xr);
        log_lx.Enqueue(xl);
        if (xr >= mawasuXRate) rxCount++;
        if (xl >= mawasuXRate) lxCount++;


        ////////////////////////////////////
        float frameTime = Time.deltaTime;
        if(wiperLNowTime > 0) wiperLNowTime -= frameTime;
        if(wiperRNowTime > 0) wiperRNowTime -= frameTime;
        if(guardLNowTime > 0) guardLNowTime -= frameTime;
        if(guardRNowTime > 0) guardRNowTime -= frameTime;
        if(jumpNowTime > 0) jumpNowTime -= frameTime;

        if(guardLflg && guardLNowTime <= guardUpTime){
            shoulderDriveSimple.guardLUp();
            guardLflg = false;
        }

        if(guardRflg && guardRNowTime <= guardUpTime){
            shoulderDriveSimple.guardRUp();
            guardRflg = false;
        }

        switch(rcvText){
            case "1":
                if(guardLNowTime <= 0 && lxCount >= mawasuXCount){
                    Debug.Log("gL");
                    guardLNowTime = guardCoolTime;
                    shoulderDriveSimple.guardLeft();
                    guardLflg = true;
                    judgeNumber = 2;
                }
                break;

            case "2":
                if(guardRNowTime <= 0 && rxCount >= mawasuXCount){
                    Debug.Log("gR");
                    guardRNowTime = guardCoolTime;
                    shoulderDriveSimple.guardRight();
                    guardRflg = true;
                    judgeNumber = 1;
                }
                break;
            
            case "3":
                if(wiperRNowTime <= 0 && wiperLNowTime <= 0 && wiperUp){
                    Debug.Log("wR");
                    wiperRNowTime = wiperCoolTime;
                    shoulderDriveSimple.wiperRight();
                    wiperUp = false;
                    judgeNumber = 3;
                }
                break;

            case "4":
                if(wiperLNowTime <= 0 && wiperRNowTime <= 0 && !wiperUp){
                    Debug.Log("wL");
                    wiperLNowTime = wiperCoolTime;
                    shoulderDriveSimple.wiperLeft();
                    wiperUp = true;
                    judgeNumber = 4;
                }
                break;

            default:
                Debug.Log("None");
                judgeNumber = 0;
            break;
        }

        
        

    }
    //データ受信時の処理
    private void ReceiveCallback(IAsyncResult ar){
        UdpClient udp = (UdpClient)ar.AsyncState;
        udp.BeginReceive(ReceiveCallback, udp);
        IPEndPoint remoteEP = null;
        byte[] rcvBytes;
        try{
            rcvBytes = udp.EndReceive(ar, ref remoteEP);
        }catch{
            Debug.Log("socket receive fault!");
            return;
        }
        rcvText = Encoding.UTF8.GetString(rcvBytes);
        Debug.Log(rcvText);
        udp.BeginReceive(ReceiveCallback, udp);
    }
    private void OnApplicationQuit(){
        //udpClient.Close();
    }

    public int GetJudgeNum()
    {
        return judgeNumber;
    }

    public bool GetLflg(){
        return guardLflg;
    }
    
    public bool GetRflg(){
        return guardRflg;
    }
}
