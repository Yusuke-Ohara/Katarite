using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistCheckPoint : MonoBehaviour
{
    public bool toggle = false;
    public string action = "nothing";

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            GameObject player = other.gameObject.transform.parent.gameObject;
            player.GetComponent<AssistViewer>().UseAssistMovie(action, toggle);

        }
    }
}
