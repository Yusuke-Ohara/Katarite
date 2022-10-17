using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutGoblet : MonoBehaviour
{
    public GameObject brokenGoblet;
    public GameObject explosionParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Instantiate(brokenGoblet, transform.position, Quaternion.identity);
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            other.gameObject.transform.parent.gameObject.GetComponent<ShoulderDriveSimple>().penartyTime = 3;
            other.gameObject.transform.parent.gameObject.GetComponent<ScoreRecorder>().boomNum++;
            other.gameObject.transform.parent.gameObject.GetComponent<Example>().JoyConRumble('B');
            Destroy(this.gameObject);
        }
    }
}
