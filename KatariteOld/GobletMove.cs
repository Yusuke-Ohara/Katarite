using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobletMove : MonoBehaviour
{

    public GameObject player;

    private bool throwFlg;
    private Rigidbody rb;

    public float throwRange;

    public float xSpeed;

    public float maxHeight;

    private float a, b, c;

    private Vector3 firstPos;

    private int targetDir;

    public GameObject gobletPoly;
 

    public float pPosErr;

    public float breakDistance;

    public GameObject crushObj;

    private float firstX;
    private float x = 0;

    public GameObject explosionEffect;
    public AudioClip sound1;
    private ShoulderDriveSimple sds;

    public AudioSource alertAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        throwFlg = false;
        sds = player.GetComponent<ShoulderDriveSimple>();
        //gobletPoly2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!throwFlg)
        {
            if(Mathf.Abs(transform.position.z - player.transform.position.z) < throwRange)
            {
                throwFlg = true;
                b = Mathf.Abs(transform.position.x - player.transform.position.x) / 2;
                c = maxHeight;
                a = -c / (b*b);
                firstPos = this.transform.position;
                if (transform.position.x - player.transform.position.x < 0)
                {
                    targetDir = 1;
                    sds.setLAlertFlg(true);
                }
                else
                {
                    targetDir = -1;
                    sds.setRAlertFlg(true);

                }
                alertAudioSource.Play();

                AssistViewer assist = player.GetComponent<AssistViewer>();
                assist.UseAssistMovie("guard", true);
                assist.ChangeAssistDir("guard", (targetDir==-1));
                //gobletPoly2.SetActive(true);
                firstX = player.transform.position.x;
                
            }
        }
        else {
            //this.transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
            gobletPoly.transform.Rotate(0, 0, 2f);
            x += targetDir * xSpeed;
            float y = firstPos.y + a * (Mathf.Abs(x) - b) * (Mathf.Abs(x) - b) + c;
            float z = player.transform.position.z + pPosErr;
            transform.position = new Vector3(firstPos.x + x + (player.transform.position.x - firstX), y, z);
            //Debug.Log("dis : "+ (transform.position - player.transform.position).magnitude);
            if((transform.position - player.transform.position).magnitude < breakDistance){
                PythonSocketRecieve psr = player.GetComponent<PythonSocketRecieve>();
                if(((targetDir == 1) ? (psr.GetLflg()) : (psr.GetRflg()))){
                    /////
                    player.GetComponent<AudioSource>().PlayOneShot(sound1);
                }else{
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    player.GetComponent<ShoulderDriveSimple>().penartyTime = 5;
                    player.GetComponent<ScoreRecorder>().boomNum++;
                    player.GetComponent<Example>().JoyConRumble((targetDir == 1) ? 'L' : 'R');
                }
                Instantiate(crushObj, transform.position, Quaternion.identity);
                //Debug.Log("crushGoblet");
                if(targetDir == 1)
                {
                    sds.setLAlertFlg(false);
                }
                else
                {
                    sds.setRAlertFlg(false);
                }
                player.GetComponent<AssistViewer>().UseAssistMovie("guard", false);
                //Debug.Log("crushGoblet2");
                Destroy(this.gameObject);
            }

        }
    }
}
