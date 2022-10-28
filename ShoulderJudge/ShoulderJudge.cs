using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShoulderJudge : MonoBehaviour
{
    private List<Joycon>    m_joycons;
    private Joycon          m_joyconL;
    private Joycon          m_joyconR;
    private Joycon.Button?  m_pressedButtonL;
    private Joycon.Button?  m_pressedButtonR;
    [SerializeField]
    private float change = 1.5f;

    // public struct y_state{
    //     double 
    // }

    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if ( m_joycons == null || m_joycons.Count <= 0 ) return;

        m_joyconL = m_joycons.Find( c =>  c.isLeft );
        m_joyconR = m_joycons.Find( c => !c.isLeft );

        double[,] x_list = new double[2,200];
        double[,] y_list = new double[2,200];
        double[,] z_list = new double[2,200];


    }
    // Update is called once per frame
    void Update()
    {
        if ( m_joycons == null || m_joycons.Count <= 0 ) return;
        if ( m_joycons == null || m_joycons.Count <= 0 )
        {
            Debug.Log( "Joy-Con が接続されていません" );
            return;
        }

        if ( !m_joycons.Any( c => c.isLeft ) )
        {
            Debug.Log( "Joy-Con (L) が接続されていません" );
            return;
        }

        if ( !m_joycons.Any( c => !c.isLeft ) )
        {
            Debug.Log( "Joy-Con (R) が接続されていません" );
            return;
        }


        var joyconL = m_joyconL;
        var joyconR = m_joyconR;

        //左のJoyConの加速度、ジャイロを取得
        var gyro_L = m_joyconL.GetGyro();
        var accel_L = m_joyconL.GetAccel();
        //右のJoyConの加速度、ジャイロを取得
        var gyro_R = m_joyconR.GetGyro();
        var accel_R = m_joyconR.GetAccel();



    }

    void judge(){

    }

    void judgeY(){

    }

    void judgeZ(){

    }

    void updatedata(){

    }
}
