using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private MovementAndAiming maa; 
    public Battery bat;

    [Header("Game Object References")]
    public GameObject mouseTracker; 

    public GameObject bulletTracer;

    [Header("Settings")]
    public float BulletSpeed = 0.5f;

    public float GunCooldown = 0.5f;

    public float SuperShotCooldown = 0.5f; 
    public Vector3 SuperShotSize = new Vector3(0f, 0.5f, 1f);
    private Parry parry; 
    private bool GunCooldownDone; 
    private bool SuperCooldownDone; 
    

    // Start is called before the first frame update
    void Start()
    {   
        maa = transform.GetComponent<MovementAndAiming>();
        GunCooldownDone = true;
        SuperCooldownDone = true;
        parry = transform.GetComponent<Parry>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !parry.Spinning){
            Debug.Log("Fired");
            Fire();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0)){
            SuperShot();
        }
    }

    Transform[] Fire(){ //returns the gameObjects that were hit 
        if (GunCooldownDone){
            bat.incrementCharge(-1); //Drain battery
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

            //Drawing the ray in the editor (doesnt show up in game)
            Debug.DrawRay(maa.child.position, mouseTracker.transform.position - maa.child.position, Color.red, 20f);
            
            RaycastHit[] hits = Physics.RaycastAll(maa.child.position, 
                mouseTracker.transform.position - maa.child.position, 
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
                       // print(enemyHealth.getHealth());
                        EnemiesHit.Add(hit.transform);
                    }
                }
            }
            Transform[] output = new Transform[EnemiesHit.Count]; 
            for (int i = 0;i < output.Length; i++){ // convert list to array
                output[i] = EnemiesHit[i];
            }
            GunCooldownDone = false;
            StartCoroutine(Cooldown());
            return output;
        }

        return null; 
    }

    public IEnumerator Cooldown(){
        yield return new WaitForSeconds(GunCooldown);
        
        GunCooldownDone = true; 
    }

    

    Transform[] SuperShot(){
        if (SuperCooldownDone && parry.CanSpinAction()){
            parry.UsedSpinAction = true;
            bat.incrementCharge(-1); //Drain battery
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

            //Drawing the ray in the editor (doesnt show up in game)
            Debug.DrawRay(maa.child.position, mouseTracker.transform.position - maa.child.position, Color.red, 20f);
            
            /*
            RaycastHit[] hits = Physics.RaycastAll(transform.position, //This one is to get the center of this raycast, and use this as the center in the boxcast
                mouseTracker.transform.position - transform.position, 
                maa.maxDistance, ~(1 << 3)); //everything hit in the path

            Vector3 center;
            center = maa.maxDistance transform.position 
            */
            print("Super Shot");

            RaycastHit[] hits = Physics.BoxCastAll(maa.child.position, SuperShotSize, 
                mouseTracker.transform.position - maa.child.position, maa.child.rotation,
                maa.maxDistance, ~(1 << 3)); //everything hit in the path
            List<Transform> EnemiesHit = new List<Transform>(); //List of specifically enemies hit, will be coverted to an array

            System.Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
            bool hitWall = false;
            //Tracer(hits[hits.Length - 1].point);
            foreach (RaycastHit hit in hits) { //look at every hit
                print(hit.transform.name);
                if (hit.transform.gameObject.layer == 6) hitWall = true;
                if (!hitWall){ //Check if we hit a wall to prevent going through
                    EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                    if (enemyHealth != null){ //If we find enemy health, lower it and then add him to the list
                        enemyHealth.incrementHealth(-1);
                        //print(enemyHealth.getHealth());
                        EnemiesHit.Add(hit.transform);
                    }
                }
            }
            Transform[] output = new Transform[EnemiesHit.Count]; 
            for (int i = 0;i < output.Length; i++){ // convert list to array
                output[i] = EnemiesHit[i];
            }
            SuperCooldownDone = false;
            StartCoroutine(SuperCooldown());
            return output;
        }

        return null; 
    }

    public IEnumerator SuperCooldown(){
        yield return new WaitForSeconds(SuperShotCooldown);

        SuperCooldownDone = true;
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
