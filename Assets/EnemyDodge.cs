using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodge : MonoBehaviour, EnemyMove
{

    public float Cooldown{get;set;}
    public float cooldownAccessor = 4;
    [Header("Dodge settings")]
    public float Distance = 5f;
    public float Force = 1000f;

    public bool Used{get;set;}

    UnityEngine.AI.NavMeshAgent agent;
    EnemyClose enemyClose; 


    public bool Act(){
        Debug.Log("Dodging");
        int DodgeDirection = 0; //1 for left, -1 for right, 0 for none (could be enum maybe)
        float temp = 0; 
        temp = Random.Range(-1f, 1f);
        if (temp > 0 ){
            DodgeDirection = 1;
        }
        else DodgeDirection = -1;

        Debug.Log(DodgeDirection);
        if (enemyClose != null && !enemyClose.GetClosing() && DodgeDirection != 0){
            
            bool cast = Physics.Raycast(transform.position, transform.right * DodgeDirection, Distance, ~(1 << 7 | 1 << 2));
            Debug.DrawRay(transform.position, transform.right * DodgeDirection, Color.red, 2f);
            if (cast) {   
                DodgeDirection = DodgeDirection * -1;
                cast = Physics.Raycast(transform.position, transform.right * DodgeDirection,
                 Distance, ~(1 << 7));
                Debug.DrawRay(transform.position, transform.right * DodgeDirection, Color.blue, 20f);
                
                if (cast) {
                    return false;
                }
            }
            agent.enabled = false;
                    
            transform.GetComponent<Rigidbody>().AddForce(transform.right * DodgeDirection * Force);
            agent.enabled = true;
        }
        
        return true;
        
    }
    public void Start(){
        Cooldown = cooldownAccessor;
        agent = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyClose = transform.GetComponent<EnemyClose>();
    }

        
      
    
       
        
}

