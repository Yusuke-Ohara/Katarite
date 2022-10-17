using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiGoalTeapMovement : MonoBehaviour
{
    private bool endFlg = false;
    private float timer;
    [SerializeField]
    private GameObject player;
    public GameObject resultCanvas;
    public GameObject GoalCanvas;
    public GameObject Kamihubuki;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        resultCanvas.SetActive(false);
        
    }

    void Update(){
        if(endFlg) timer += Time.deltaTime;
        if(timer > 5){
            //SceneManager.LoadScene(2);
            GoalCanvas.SetActive(false);
            player.GetComponent<Rigidbody>().isKinematic = true;
            resultCanvas.SetActive(true);
            resultCanvas.GetComponent<ResultViewer>().ShowResult();
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider target){
        if(target.gameObject.tag == "Player"){
            GoalCanvas.SetActive(true);
            Kamihubuki.SetActive(true);
            //player = target.gameObject.transform.parent.gameObject;
            player.GetComponent<ScoreRecorder>().SaveResult();
            endFlg = true;

        }
    }
}