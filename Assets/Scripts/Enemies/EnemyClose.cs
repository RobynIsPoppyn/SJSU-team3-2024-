using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyClose : MonoBehaviour, EnemyMove
{
    public float Cooldown{get; set;}
    public bool Used{get; set;}
    public float cooldownAccessor;
    [Header("Close In Properties")]
    public float CloseLength = 2f;
    public float timeForClosing = 3f;
    
    bool Closing = false;

    NavMeshAgent navMesh;
    public void Start()
    {
        Cooldown = cooldownAccessor;
        navMesh = transform.GetComponent<NavMeshAgent>();
    }

    
    public bool Act(){
        if (navMesh.enabled == true){
            Transform target = transform.GetComponent<EnemyAI>().target;
            Debug.Log("Closing in");
            float distance = Vector3.Distance(target.position, transform.position);
            float temp = navMesh.stoppingDistance - CloseLength;
            float oldDistance = navMesh.stoppingDistance; 
            if (temp < 0 ) navMesh.stoppingDistance = 0; //Set stopping distance to the Close length or zero if it exceeds
            else navMesh.stoppingDistance -= temp;
            StartCoroutine(ActHelper(oldDistance));
        }
        return true;
    }

    public IEnumerator ActHelper(float distance){
        Closing = true;
        yield return new WaitForSeconds(timeForClosing);
        Closing = false;
        navMesh.stoppingDistance = distance;

    }

    public bool GetClosing(){
        return Closing;
    }


}
