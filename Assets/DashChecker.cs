using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashChecker : MonoBehaviour
{
    //This scripts shows if there are any collisions so that the player doesnt dash through walls

    public bool Colliding {get; private set;}

    public void OnTriggerStay(Collider other){
        
        if (other.transform.tag.Equals("Wall")){
            
            Colliding = true;
        }
    }
    public void OnTriggerExit(Collider other){
        if (other.transform.tag.Equals("Wall")){
            Colliding = false;
        }
    }
    
}
