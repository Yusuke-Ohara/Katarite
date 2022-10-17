using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoalTeapMove : MonoBehaviour
{
    private bool endFlg = false;
    private float timer;
    [SerializeField]
    private GameObject player;
    public GameObject resultCanvas;
    public GameObject GoalCanvas;
    public GameObject Kamihubuki;

    [SerializeField]
    private GameType gameType;

    public AudioClip GoalSound;
    [SerializeField]
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        resultCanvas.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update(){
        if(endFlg) timer += Time.deltaTime;
        if(timer > 5){
            //SceneManager.LoadScene(2);
            GoalCanvas.SetActive(false);
            if(gameType == GameType.ShoulderDrive) player.GetComponent<Rigidbody>().isKinematic = true;
            else if(gameType == GameType.ShoulderSki){
                player.GetComponent<Example>().hundleflag = false;
                player.GetComponent<skiing_movement>().Gravity_zero();
            }
            resultCanvas.SetActive(true);
            resultCanvas.GetComponent<ResultViewer>().ShowResult();
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider target){
        if(target.gameObject.tag == "Player"){
            GoalCanvas.SetActive(true);
            Kamihubuki.SetActive(true);
            if(gameType == GameType.ShoulderDrive) player.GetComponent<ScoreRecorder>().SaveResult();
            else if(gameType == GameType.ShoulderSki) player.GetComponent<ScoreRecorde_Skiing>().SaveResult();
            endFlg = true;
            audioSource.PlayOneShot(GoalSound);

        }
    }
    public bool Give_endFlg()
    {
        return endFlg;
    }
}
