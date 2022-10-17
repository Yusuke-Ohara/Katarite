using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test_gyro : MonoBehaviour
{
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues( typeof( Joycon.Button ) ) as Joycon.Button[];
    private List<Joycon>    m_joycons;
    private Joycon          m_joyconL;
    private Joycon          m_joyconR;
    private Joycon.Button?  m_pressedButtonL;
    private Joycon.Button?  m_pressedButtonR;
    private float R_x, R_y, R_z, L_x, L_y, L_z, maxR_x, maxR_y, maxR_z, minR_x, minR_y, minR_z, maxL_x, maxL_y, maxL_z, minL_x, minL_y, minL_z;
    private float normalR_x, normalR_y, normalR_z, normalL_x, normalL_y, normalL_z, sumnormalR_x, sumnormalR_y, sumnormalR_z, sumnormalL_x, sumnormalL_y, sumnormalL_z;
    private int updown, hineru, kaiten = 0; //ボタンのフラグ
    bool showGUI = true, result;　//リアルタイムの値と結果の表示の切り替え
    public Text textR; //各動作の結果（R)表示用テキスト：Text_R
    public Text textL; //各動作の結果（L)表示用テキスト：Text_L
    public Text text1; //ボタン”updown”のテキスト
    public Text text2; //ボタン”hineri”のテキスト
    public Text text3; //ボタン”kaiten”のテキスト
    public Text text4; //ボタン”Stop”のテキスト
    public Text All; //最終結果表示用テキスト：All
    float[] array = {-100,-100,-100,100,100,100,-100,-100,-100,100,100,100};
    float[] reset_array = {-100,-100,-100,100,100,100,-100,-100,-100,100,100,100};


    // Start is called before the first frame update
    void Start()
    { 
        m_joycons = JoyconManager.Instance.j;

        if ( m_joycons == null || m_joycons.Count <= 0 ) return;

        m_joyconL = m_joycons.Find( c =>  c.isLeft );
        m_joyconR = m_joycons.Find( c => !c.isLeft );

        text1.text = "②上げ下げ";
        text2.text = "③ひねる";
        text3.text = "④回す";

        result = false;
        Reset_data();
        maxR_x = -100;
        maxL_x = -100;
        minR_x = 100;
        minL_x = 100;

        maxR_y = -100;
        maxL_y = -100;
        minR_y = 100;
        minL_y = 100;

        sumnormalR_x = 0;
        sumnormalR_y = 0;
        sumnormalR_z = 0;
        sumnormalL_x = 0;
        sumnormalL_y = 0; 
        sumnormalL_z = 0;
　　　　 
    }


    // Update is called once per frame
    void Update()
    {
        var gyro_R = m_joyconR.GetGyro();
        var gyro_L = m_joyconL.GetGyro();
        R_x = gyro_R.x;
        R_y = gyro_R.y;
        R_z = gyro_R.z;
        L_x = gyro_L.x;
        L_y = gyro_L.y; 
        L_z = gyro_L.z;

        if(updown == 1){
            Compare();
        }

        if(hineru == 1){
            Compare();
        }

        if(kaiten == 1){
            Compare();
        }
    }


    void Compare(){
        if(maxR_x < R_x){
            maxR_x = R_x;
        }else if(minR_x > R_x){
            minR_x = R_x;
        }

        if(maxR_y < R_y){
            maxR_y = R_y;
        }else if(minR_y > R_y){
            minR_y = R_y;
        }

        if(maxR_z < R_z){
            maxR_z = R_z;
        }else if(minR_z > R_z){
            minR_z = R_z;
        }

        if(maxL_x < L_x){
            maxL_x = L_x;
        }else if(minL_x > L_x){
            minL_x = L_x;
        }

        if(maxL_y < L_y){
            maxL_y = L_y;
        }else if(minL_y > L_y){
            minL_y = L_y;
        }

        if(maxL_z < L_z){
            maxL_z = L_z;
        }else if(minL_z > L_z){
            minL_z = L_z;
        }
    }


    void Compare_all(){
        if(array[0] <= maxR_x){
            array[0] = maxR_x;
        }
        
        if(array[3] > minR_x){
            array[3] = minR_x;
        }

        if(array[1] <= maxR_y){
            array[1] = maxR_y;
        }
        
        if(array[4] > minR_y){
            array[4] = minR_y;
        }

        if(array[2] <= maxR_z){
            array[2] = maxR_z;
        }
        
        if(array[5] > minR_z){
            array[5] = minR_z;
        }

        if(array[6] <= maxL_x){
            array[6] = maxL_x;
        }
        
        if(array[9] > minL_x){
            array[9] = minL_x;
        }

        if(array[7] <= maxL_y){
            array[7] = maxL_y;
        }
        
        if(array[10] > minL_y){
            array[10] = minL_y;
        }

        if(array[8] <= maxL_z){
            array[8] = maxL_z;
        }
        
        if(array[11] > minL_z){
            array[11] = minL_z;
        }

        sumnormalR_x += normalR_x;
        sumnormalR_y += normalR_y;
        sumnormalR_z += normalR_z;
        sumnormalL_x += normalL_x;
        sumnormalL_y += normalL_y;
        sumnormalL_z += normalL_z;
    }


    void Reset_data(){
        maxR_x = -100;
        maxR_y = -100;
        maxR_z = -100;
        minR_x = 100;
        minR_y = 100;
        minR_z = 100;
        maxL_x = -100;
        maxL_y = -100;
        maxL_z = -100;
        minL_x = 100;
        minL_y = 100;
        minL_z = 100;
        normalR_x = 0;
        normalR_y = 0;
        normalR_z = 0;
        normalL_x = 0;
        normalL_y = 0; 
        normalL_z = 0;
    }


    public void Updown(){
        print("上げ下げ");
        if(updown != 1){
            updown = 1;
            text1.text = "止める";
            normalR_x = R_x;
            normalR_y = R_y;
            normalR_z = R_z;
            normalL_x = L_x;
            normalL_y = L_y;
            normalL_z = L_z;
        }else if(updown == 1){
            updown = 0;
            text1.text = "②上げ下げ";
            Stop();
        }
    }


    public void Hineru(){
        print("ひねる");
        if(hineru != 1){
            hineru = 1;
            text2.text = "止める";
            normalR_x = R_x;
            normalR_y = R_y;
            normalR_z = R_z;
            normalL_x = L_x;
            normalL_y = L_y;
            normalL_z = L_z;
        }else if(hineru == 1){
            hineru = 0;
            text2.text = "③ひねる";
            Stop();
        }
    }


    public void Kaiten(){
        print("回す");
        if(kaiten != 1){
            kaiten = 1;
            text3.text = "止める";
            normalR_x = R_x;
            normalR_y = R_y;
            normalR_z = R_z;
            normalL_x = L_x;
            normalL_y = L_y;
            normalL_z = L_z;
        }else if(kaiten == 1){
            kaiten = 0;
            text3.text = "④回す";
            Stop();
        }
    }


    public void Stop(){
        textR.text = "maxR_x: " + maxR_x + "\n" + "maxR_y: " + maxR_y + "\n" + "maxR_z: " + maxR_z + "\n" 
                       + "minR_x: " + minR_x + "\n" + "minR_y: " + minR_y + "\n" + "minR_z: " + minR_z + "\n"
                       + "normalR_x: " + normalR_x + "\n" + "normalR_y: " + normalR_y + "\n" + "normalR_z: " + normalR_z;
            
        textL.text = "maxL_x: " + maxL_x + "\n" + "maxL_y: " + maxL_y + "\n" + "maxL_z: " + maxL_z + "\n"
                       + "minL_x: " + minL_x + "\n" + "minL_y: " + minL_y + "\n" + "minL_z: " + minL_z + "\n"
                       + "normalL_x: " + normalL_x + "\n" + "normalL_y: " + normalL_y + "\n" + "normalL_z: " + normalL_z;

        print("\n" + "maxR_x: " + maxR_x + "\n" + "maxR_y: " + maxR_y + "\n" + "maxR_z: " + maxR_z + "\n" 
                    + "minR_x: " + minR_x + "\n" + "minR_y: " + minR_y + "\n" + "minR_z: " + minR_z + "\n"
                    + "maxL_x: " + maxL_x + "\n" + "maxL_y: " + maxL_y + "\n" + "maxL_z: " + maxL_z + "\n"
                    + "minL_x: " + minL_x + "\n" + "minL_y: " + minL_y + "\n" + "minL_z: " + minL_z + "\n"
                    + "normalR_x: " + normalR_x + "\n" + "normalR_y: " + normalR_y + "\n" + "normalR_z: " + normalR_z + "\n"
                    + "normalL_x: " + normalL_x + "\n" + "normalL_y: " + normalL_y + "\n" + "normalL_z: " + normalL_z);

        Compare_all();
        Reset_data();
    }


    public void Show_result(){
        if(result){
            result = false;
            showGUI = false;
            text4.text = "①リセット";
            textR.text = "";
            textL.text = "";
            All.text = "maxR_x: " + array[0] + "\n" + "maxR_y: " + array[1] + "\n" + "maxR_z: " + array[2] + "\n" 
                        + "minR_x: " + array[3] + "\n" + "minR_y: " + array[4] + "\n" + "minR_z: " + array[5] + "\n"
                        + "maxL_x: " + array[6] + "\n" + "maxL_y: " + array[7] + "\n" + "maxL_z: " + array[8] + "\n"
                        + "minL_x: " + array[9] + "\n" + "minL_y: " + array[10] + "\n" + "minL_z: " + array[11] + "\n"
                        + "avenormalR_x: " + sumnormalR_x / 3 + "\n" + "avenormalR_y: " + sumnormalR_y / 3 + "\n" + "avenormalR_z: " + sumnormalR_z / 3 + "\n"
                        + "avenormalL_x: " + sumnormalL_x / 3 + "\n" + "avenormalL_y: " + sumnormalL_y / 3 + "\n" + "avenormalL_z: " + sumnormalL_z / 3;
        }else{
            result = true;
            showGUI = true;
            text4.text = "⑤完了";
            All.text = "";
            Reset_data();
            for(int i = 0 ; i < 12; i++){
                array[i] = reset_array[i];
            }
            sumnormalR_x = 0;
            sumnormalR_y = 0;
            sumnormalR_z = 0;
            sumnormalL_x = 0;
            sumnormalL_y = 0; 
            sumnormalL_z = 0;
            PlayerPrefs.DeleteAll();
        }
    }

    public void NextScene(){    
		SceneManager.LoadScene("shoulderJudgeTest");
    }


    private void OnGUI()
    {
        if(showGUI){
            var style = GUI.skin.GetStyle( "label" );
            style.fontSize = 24;

            if ( m_joycons == null || m_joycons.Count <= 0 )
            {
                GUILayout.Label( "Joy-Con が接続されていません" );
                return;
            }

            if ( !m_joycons.Any( c => c.isLeft ) )
            {
                GUILayout.Label( "Joy-Con (L) が接続されていません" );
                return;
            }

            if ( !m_joycons.Any( c => !c.isLeft ) )
            {
                GUILayout.Label( "Joy-Con (R) が接続されていません" );
                return;
            }

            GUILayout.BeginHorizontal( GUILayout.Width( 960 ) );

            foreach ( var joycon in m_joycons )
            {
                var isLeft      = joycon.isLeft;
                var name        = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
                var key         = isLeft ? "Z キー" : "X キー";
                var button      = isLeft ? m_pressedButtonL : m_pressedButtonR;
                // var stick       = joycon.GetStick();
                var gyro        = joycon.GetGyro();
                // var accel       = joycon.GetAccel();
                // var orientation = joycon.GetVector();


                GUILayout.BeginVertical( GUILayout.Width( 480 ) );
                GUILayout.Label( name );
                // GUILayout.Label( key + "：振動" );
                // GUILayout.Label( "押されているボタン：" + button );
                // GUILayout.Label( string.Format( "スティック：({0}, {1})", stick[ 0 ], stick[ 1 ] ) );
                // GUILayout.Label( "ジャイロ：" + gyro );
                GUILayout.Label( "加速度x：" + (Mathf.Floor(gyro.x*100)) / 100 );
                GUILayout.Label( "加速度y：" + (Mathf.Floor(gyro.y*100)) / 100 );
                GUILayout.Label( "加速度z：" + (Mathf.Floor(gyro.z*100)) / 100 );
                // GUILayout.Label( "傾き：" + orientation );
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }
}