using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム中，車が進行不能時に最も近いセーブポイントに戻る（ShoulderDrive）
public class Respown : MonoBehaviour
{
    public GameObject respown; //リスポーン地点
    public GameObject startpoint; //スタート地点
    public List<GameObject> goalPoint = new List<GameObject>(); //ゴール地点（難易度ごとに違うためリスト）
    // [SerializeField]
    // private GameObject goaltpoint; //ゴール地点
    private Rigidbody rb; //車のリジッドボディ
    public bool nowGame = false; //ゲーム中かどうか
    [SerializeField]
    private float stoptime; //停止時間
    private float stopv = 0.05f; 

    private int level; //難易度

    public GameObject FlyEnemy;


    [SerializeField]
    private string Objectname;


    void Start()
    {
        respown = startpoint;
        rb = this.transform.GetComponent<Rigidbody>();
        level = PlayerPrefs.GetInt("Drive_level",1);
        FlyEnemy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (nowGame) //ゲーム中のみ
        {
            if(!FlyEnemy.activeSelf){
                FlyEnemy.SetActive(true);
            }
            

            Objectname = respown.name;
            if (Input.GetKeyDown(KeyCode.O))
            {
                RespownPos(respown);
            }
            
            //停止時間のカウント
            if(rb.velocity.magnitude <= stopv) 
            {
                stoptime += Time.deltaTime;
            }

            // 停止時間3秒でリスポーン
            if (stoptime >= 3.0)
            {
                stoptime = 0;
                RespownPos(respown);
            }

            // スタート地点に移動
            if (Input.GetKeyDown(KeyCode.S))
            {
                nowGame = false;
                RespownPos(startpoint);

            }

            // ゴールに移動
            if (Input.GetKeyDown(KeyCode.G))
            {
                RespownPos(goalPoint[level]);
            }

        }
        

    }

    //セーブポイントを通ったら、リスポーン地点を更新
    //ゴール地点通過：ゲーム外
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("savepoint"))
        {
            respown = other.gameObject;
        }
        if (other.CompareTag("goal"))
        {
            nowGame = false;
        }
    }

    //スタート地点を通：ゲーム中　
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("start"))
        {
            nowGame = true;
        }
    }

    //最後に通過したリスポーン地点に移動
    public void RespownPos(GameObject rs)
    {
        // Debug.Log("respown");
        rb.velocity = Vector3.zero;
        Vector3 respownPos;
        respownPos = rs.transform.position;
        respownPos.y = rs.transform.position.y+0.5f;
        this.transform.position = respownPos;
        this.transform.rotation = rs.transform.rotation;
        this.transform.Rotate(new Vector3(0,90,0));
    }

}
