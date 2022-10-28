using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 難易度によるギミック（樽，ジャンプ台，ダッシュアイテム，FlyEnemmy）の変更（ShoulderDrive）
public class GimmickManager : MonoBehaviour
{

    private int level;
    public List<GameObject> GimmlickLevel = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        level = PlayerPrefs.GetInt("Drive_level",1);
        for(int i = 0; i < 3; i++){
            if(i == level){
                GimmlickLevel[i].SetActive(true);
            }else{
                GimmlickLevel[i].SetActive(false);
            }
        }
        
    }
}
