using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Joy-Con未接続時に状態提示（タイトル画面）
public class JoyconD : MonoBehaviour
{

    public List<Joycon>    m_joycons;

    public Image controlerL;
    public Image controlerR;
    public GameObject disconnectL;
    public GameObject disconnectR;
    public bool joyConnect;


    void Start()
    {
        // 接続されているJoy-Conの情報格納
        m_joycons = JoyconManager.Instance.j;

        // Joy-Conの接続数，未接続のJoy-Con（左右）を提示
        if ( m_joycons == null || m_joycons.Count == 0){
            controlerL.color = new Color(49f / 255f, 49f / 255f, 49f / 255f);
            controlerR.color = new Color(49f / 255f, 49f / 255f, 49f / 255f);
            disconnectL.SetActive(true);
            disconnectR.SetActive(true);
            joyConnect = false;
            return;
        }else if(m_joycons.Count == 1 ){
            joyConnect = false;
            if(m_joycons[0].isLeft){
                controlerR.color = new Color(49f / 255f, 49f / 255f, 49f / 255f);
                disconnectR.SetActive(true);
                return;
            }else{
                controlerL.color = new Color(49f / 255f, 49f / 255f, 49f / 255f);
                disconnectL.SetActive(true);
                return;
            }
        }else{
            joyConnect = true;
        }
    }
}
