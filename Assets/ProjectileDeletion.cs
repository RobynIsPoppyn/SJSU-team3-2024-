using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDeletion : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider collisionInfo){
        
        if (collisionInfo.transform.tag.Equals("Wall")){
            
            GameObject.Destroy(transform.gameObject);
        }
    }
}
