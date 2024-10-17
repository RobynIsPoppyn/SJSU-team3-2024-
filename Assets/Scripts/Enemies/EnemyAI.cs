using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public float lookRadius = 10f;
    public Transform target;
    NavMeshAgent agent;

    //int movePoints = 100; //track the avaialble points an enemy has to use a move, which drains depending on the move
    //refills continously (update??)
    
    List<EnemyMove> MoveList = new List<EnemyMove>();
    //refresh test
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        foreach (EnemyMove x in gameObject.GetComponents<EnemyMove>()){ //Add every move component in an enemy to this list
            MoveList.Add(x);
        }
      //s  attack = GetComponent<EnemyAttack>();
    }

    private bool performed;

    // Update is called once per frame
    private float timeSinceAbility; //prevent ability calls from being spammed
    void Update()
    {
        timeSinceAbility += Time.deltaTime; 
        float distance = Vector3.Distance(target.position, transform.position); //distance between the target and player
        if (distance <= lookRadius && agent.enabled){ //If the enemy is in range and the nav agent is working
            agent.SetDestination(target.position);
            FaceTarget(); 
            if (distance <= agent.stoppingDistance && timeSinceAbility >= 0.1f){ //Once the enemy is close enough to the player that it stops (might be changed)
                EnemyMove moveSelect = selectMove();  
                if (moveSelect != null){
                    moveSelect.Act();
                    timeSinceAbility = 0f; //reset timer -- we can also adjust this through certain abilities maybe to lock out other other abilities
                    
                    StartCoroutine(moveCooldown(moveSelect)); //set this indivual move on cooldown
                }
              
            }

        }
    }

    void FaceTarget(){ //Shifts rotation to the player, but goes overtime 
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public EnemyMove selectMove(){ //Randomly pick a move from the enemy move list
        float distance = Vector3.Distance(target.position, transform.position);
        int output = Random.Range(0, MoveList.Count);
        if (MoveList[output].Used){ //Since each move has an independent cooldown, this staggers everything
            return null; 
        }
        

        return MoveList[output];
    }

    public IEnumerator moveCooldown(EnemyMove em){ 
        em.Used = true;
        yield return new WaitForSeconds(em.Cooldown);

        em.Used = false;
    

        

    }
}
