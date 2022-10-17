using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 画像の移動（タイトル画面）
public class ImgMove : MonoBehaviour
{
    public float maxPos = -100; //MAX上昇位置
    public float minPos = -112; //MIN下降位置
    public float movespeed = 6.0f; //移動速度
    bool max = false;
    bool min = false;
    int num = 1;

    private Vector3 pos;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(transform.localPosition.y);

        pos = transform.localPosition;

        transform.Translate(new Vector3(0,movespeed*num,0) * Time.deltaTime);

        if(pos.y > maxPos)
        {
            num = -1;
        }
        if(pos.y < minPos)
        {
            num = 1;
        }
    }
}
