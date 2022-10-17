using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using UnityEngine.UI;

public class PythonSocketForExp : MonoBehaviour
{ // Start is called before the first frame update
    static UdpClient udpClient;
    IPEndPoint remoteEP = null;

    public float wiperCoolTime;
    public float guardCoolTime;

    public float jumpCoolTime;

    public float guardUpTime;

    [SerializeField] private float wiperLNowTime, wiperRNowTime, guardLNowTime, guardRNowTime, jumpNowTime;

    private bool wiperUp, guardLflg, guardRflg;


    public int logLength;

    private Queue<float> log_rx, log_lx;
    private int rxCount, lxCount;

    public float mawasuXRate;

    public int mawasuXCount;

    private string rcvText;

    private JoyconData joyconData;

    //UI�n
    public Toggle JumpT;
    public Toggle LeftWiperT;
    public Toggle RightWiperT;
    public Toggle LeftGuardT;
    public Toggle RightGuardT;


    void Start()
    {
        log_lx = new Queue<float>();
        log_rx = new Queue<float>();
        int LOCA_LPORT = 50007;
        udpClient = new UdpClient(LOCA_LPORT);
        udpClient.BeginReceive(ReceiveCallback, udpClient);

        wiperLNowTime = 0;
        wiperRNowTime = 0;
        guardLNowTime = 0;
        guardRNowTime = 0;
        jumpNowTime = 0;
        wiperUp = false;
        guardLflg = guardRflg = false;

        rxCount = 0;
        lxCount = 0;

        for (int i = 0; i < logLength; i++)
        {
            log_rx.Enqueue(0);
            log_lx.Enqueue(0);
        }

        joyconData = GetComponent<JoyconData>();
    }

    // Update is called once per frame

    void Update()
    {

        //�񂷔���pxlog�쐬
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
        if (wiperLNowTime > 0) wiperLNowTime -= frameTime;
        if (wiperRNowTime > 0) wiperRNowTime -= frameTime;
        if (guardLNowTime > 0) guardLNowTime -= frameTime;
        if (guardRNowTime > 0) guardRNowTime -= frameTime;
        if (jumpNowTime > 0) jumpNowTime -= frameTime;

        if (guardLflg && guardLNowTime <= guardUpTime)
        {
            guardLflg = false;
        }

        if (guardRflg && guardRNowTime <= guardUpTime)
        {
            guardRflg = false;
        }

        switch (rcvText)
        {
            case "1":
                if (guardLNowTime <= 0 && lxCount >= mawasuXCount)
                {
                    Debug.Log("gL"+lxCount);
                    guardLNowTime = guardCoolTime;
                    guardLflg = true;
                    LeftGuardT.isOn = true;

                }
                break;

            case "2":
                if (guardRNowTime <= 0 && rxCount >= mawasuXCount)
                {
                    Debug.Log("gR"+rxCount);
                    guardRNowTime = guardCoolTime;
                    guardRflg = true;
                    RightGuardT.isOn = true;
                }
                break;

            case "3":
                if (wiperRNowTime <= 0 && wiperLNowTime <= 0 && rxCount < mawasuXCount)
                {
                    Debug.Log("wR");
                    wiperRNowTime = wiperCoolTime;
                    wiperUp = false;
                    RightWiperT.isOn = true;
                }
                break;

            case "4":
                if (wiperLNowTime <= 0 && wiperRNowTime <= 0 && lxCount < mawasuXCount)
                {
                    Debug.Log("wL");
                    wiperLNowTime = wiperCoolTime;
                    wiperUp = true;
                    LeftWiperT.isOn = true;
                }
                break;

            default:
                // Debug.Log("None");
                break;
        }




    }
    //�f�[�^��M���̏���
    private void ReceiveCallback(IAsyncResult ar)
    {
        UdpClient udp = (UdpClient)ar.AsyncState;
        udp.BeginReceive(ReceiveCallback, udp);
        IPEndPoint remoteEP = null;
        byte[] rcvBytes;
        try
        {
            rcvBytes = udp.EndReceive(ar, ref remoteEP);
        }
        catch
        {
            Debug.Log("socket receive fault!");
            return;
        }
        rcvText = Encoding.UTF8.GetString(rcvBytes);
        Debug.Log(rcvText);
        udp.BeginReceive(ReceiveCallback, udp);
    }
    private void OnApplicationQuit()
    {
        udpClient.Close();
    }

    public void resetButton()
    {
        JumpT.isOn = false;
        LeftWiperT.isOn = false;
        RightWiperT.isOn = false;
        LeftGuardT.isOn = false;
        RightGuardT.isOn = false;
    }
}
