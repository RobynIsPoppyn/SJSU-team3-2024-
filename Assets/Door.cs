using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private LevelManager lm;

    public void Start(){lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();}
    public void OnTriggerEnter(Collider collider){
        if (collider.tag == "Player"){
            print("DoorHit");
            lm.LoadNext();
        }
    }
}
