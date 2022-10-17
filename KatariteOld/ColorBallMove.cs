using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBallMove : MonoBehaviour
{

    private float nowTime;
    public float throwTime;

    public float distance;

    public float ballSpeed;

    private bool throwFlg;

    private Renderer renderer;

    private Vector3 targetVec;

    private Rigidbody rigidbody;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        nowTime = 900;
        throwFlg = false;
        renderer = GetComponent<Renderer>();
        renderer.material.color -= new Color(0,0,0,1f);
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(throwFlg && nowTime <= 0){
            renderer.material.color += new Color(0,0,0,1f);
            throwFlg = false;
            targetVec = player.transform.position - this.transform.position + new Vector3(0,0,distance);
            targetVec.y /= 3.0f;
            rigidbody.isKinematic = false;
            rigidbody.velocity = targetVec.normalized * ballSpeed;
        }else if(throwFlg){
            nowTime -= Time.deltaTime;
        }else if(nowTime <= 0){
            rigidbody.velocity = targetVec.normalized * ballSpeed; 
            if((player.transform.position - this.transform.position).magnitude <= 2f){
                ShoulderDriveSimple sds = player.GetComponent<ShoulderDriveSimple>();
                sds.paintHit();
                AssistViewer assist = player.GetComponent<AssistViewer>();
                assist.UseAssistMovie("wiper", true);
                assist.ChangeAssistDir("wiper", sds.wiperIsUp);
                //this.SetActive(false);
            }
        } 
    }

    public void throwStart(){
        nowTime = throwTime;
        throwFlg = true;

    }
}
