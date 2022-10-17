using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// マウスカーソルの表示制御
public class Mouse : MonoBehaviour
{
    void Start()
    {
        // アプリ起動時に非表示
        Cursor.visible = false;
    }

    void Update()
    {
        // 左CTRLキーで表示切り替え
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            if(Cursor.visible){
                Cursor.visible = false;
            }else{
                Cursor.visible = true;
            }
        }
    }
}
