using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joy-Conの接続状況の提示（ゲームシーン）
public class JoyDis : MonoBehaviour
{
    public List<Joycon>    m_joycons;
    public GameObject DisJoy;
    public bool joyConnect;

    // Start is called before the first frame update
    void Start()
    {
        m_joycons = JoyconManager.Instance.j;
        // Debug.Log(m_joycons.Count);
        // controlerL.GetComponent<Image>().color = new Color(49f / 255f, 49f / 255f, 49f / 255f);

        if ( m_joycons == null || m_joycons.Count <= 1){
            joyConnect = false;
            DisJoy.SetActive(true);
            return;
        }else{
            joyConnect = true;
        }
    }
}
