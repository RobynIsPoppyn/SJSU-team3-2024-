using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public MovementAndAiming maa; 
    public Battery bat;
    public GameObject mouseTracker; 

    public GameObject bulletTracer;

    public float BulletSpeed = 0.5f;

    // healthsystem variable
    public healthSystem hs;



    // Start is called before the first frame update
    void Start()
    {
        hs = FindObjectOfType<healthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            print("Fired");
            Fire();

            //health system check
            hs.takeDamage(1);
        }
    }

    Transform[] Fire(){ //returns the gameObjects that were hit 

        bat.incrementCharge(-1); //Drain battery
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

        //Drawing the ray in the editor (doesnt show up in game)
        Debug.DrawRay(transform.position, mouseTracker.transform.position - transform.position, Color.red, 20f);
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, 
            mouseTracker.transform.position - transform.position, 
            maa.maxDistance, ~(1 << 3)); //everything hit in the path
        List<Transform> EnemiesHit = new List<Transform>(); //List of specifically enemies hit, will be coverted to an array

        System.Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
        bool hitWall = false;
        Tracer(hits[hits.Length - 1].point);
        foreach (RaycastHit hit in hits) { //look at every hit
            print(hit.transform.name);
            if (hit.transform.gameObject.layer == 6) hitWall = true;
            if (!hitWall){ //Check if we hit a wall to prevent going through
                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                if (enemyHealth != null){ //If we find enemy health, lower it and then add him to the list
                    enemyHealth.incrementHealth(-1);
                    print(enemyHealth.getHealth());
                    EnemiesHit.Add(hit.transform);
                }
            }
        }
        Transform[] output = new Transform[EnemiesHit.Count]; 
        for (int i = 0;i < output.Length; i++){ // convert list to array
            output[i] = EnemiesHit[i];
        }
        return output;
    }

    void Tracer(Vector3 lastHit){
        
        GameObject cloned = Instantiate(bulletTracer);
        cloned.transform.position = transform.position;
        cloned.SetActive(true);


        StartCoroutine(TracerHelper(cloned, transform.position, lastHit, 0));

    }

    IEnumerator TracerHelper(GameObject cloned, Vector3 startPosition, Vector3 lastHit, int Counter){
        Rigidbody rb = cloned.GetComponent<Rigidbody>();
        if (cloned.transform.position.Equals(lastHit) || Counter >= 5){
            Destroy(cloned);
            Debug.Log("Gone");
            yield return new WaitForSeconds(0f);
        }
        else{
           
            startPosition = Vector3.Lerp(startPosition, lastHit, BulletSpeed);
            rb.MovePosition(startPosition);
        
            yield return new WaitForSeconds(0.05f); 
            
            StartCoroutine(TracerHelper(cloned, startPosition, lastHit, Counter + 1));
        }

    }
}
