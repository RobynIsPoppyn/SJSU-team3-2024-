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
    public GameObject superTracer;
    public GameObject shotPoint;
    public GameObject body; 
    public Animator handAnim; 

    [Header("Settings")]
    public float BulletSpeed = 0.5f;

    public float GunCooldown = 0.5f;
    public float GunOffset = 1.5f;
    public int gunSelfDamage = 1;
    public AudioSource shotSound;
    public AudioSource superSound;

    public float SuperShotCooldown = 0.5f; 
    public Vector3 SuperShotSize = new Vector3(0f, 0.5f, 1f);
    public float SuperOffset = 2f;
    public int superSelfDamage = 4;

    public float TurnSpeedRadians;
    public float MaxVector;
    public float TurnUpdateRate;


    //privates
    private Parry parry; 
    private bool GunCooldownDone; 
    private bool SuperCooldownDone; 
    private CameraFollow camFollow; 
    private healthSystem hs;
    
    
    

    // Start is called before the first frame update
    void Start()
    {   
        maa = transform.GetComponent<MovementAndAiming>();
        GunCooldownDone = true;
        SuperCooldownDone = true;
        parry = transform.GetComponent<Parry>();
        camFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        hs = transform.GetComponent<healthSystem>();
        mouseTracker = GameObject.Find("MouseTracker");
        shotSound = GameObject.Find("GunNormalShot").GetComponent<AudioSource>();
        superSound = GameObject.Find("GunSuperShot").GetComponent<AudioSource>();
    }

    float TurnTimer = 2f;
    Vector3 shotEuler;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !parry.Spinning && !maa.Dead){
            Debug.Log("Fired");
            Fire();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && !maa.Dead){
            SuperShot();
        }

        TurnTimer += Time.deltaTime;
        if (TurnTimer >= TurnUpdateRate){
            shotEuler = shotPoint.transform.eulerAngles;
            TurnTimer = 0;
        }
        if (!maa.Dead)
        TurnBody(maa.mouseTarget());

        
        
    }

    Transform[] Fire(){ //returns the gameObjects that were hit 
        if (GunCooldownDone){
            handAnim.SetTrigger("Shoot");
            shotSound.Play();
            hs.takeDamage(gunSelfDamage);
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            camFollow.GunShake();
            //Drawing the ray in the editor (doesnt show up in game)
            Debug.DrawRay(maa.child.position, mouseTracker.transform.position - maa.child.position, Color.red, 20f);
            
            RaycastHit[] hits = Physics.RaycastAll(shotPoint.transform.position, 
                mouseTracker.transform.position - shotPoint.transform.position, 
                maa.maxDistance, ~(1 << 3)); //everything hit in the path
            List<Transform> EnemiesHit = new List<Transform>(); //List of specifically enemies hit, will be coverted to an array

            System.Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
            System.Array.Reverse(hits);
            bool hitWall = false;
            Tracer(hits[hits.Length - 1].point + Vector3.up * GunOffset, bulletTracer);
            foreach (RaycastHit hit in hits) { //look at every hit
                print(hit.transform.name);
                if (hit.transform.gameObject.layer == 6) hitWall = true;
                if (!hitWall){ //Check if we hit a wall to prevent going through
                    EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                    if (enemyHealth != null){ //If we find enemy health, lower it and then add him to the list
                        enemyHealth.takeDamage(1);
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
        bool stopspintemp = parry.StopSpin(false);
      //  print(parry.CanSpinAction() + "  " + stopspintemp);
        if (parry.CanSpinAction() && stopspintemp){
            parry.isInvincible = false;
            hs.takeDamage(superSelfDamage);
            handAnim.SetTrigger("Shoot");
            superSound.Play();
            Parry.SpinCounter++;
            //parry.UsedSpinAction = true;
            camFollow.SuperShake();
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


            RaycastHit[] hits = Physics.BoxCastAll(shotPoint.transform.position, SuperShotSize, 
                mouseTracker.transform.position - shotPoint.transform.position, maa.child.rotation,
                maa.maxDistance, ~(1 << 3)); //everything hit in the path
            List<Transform> EnemiesHit = new List<Transform>(); //List of specifically enemies hit, will be coverted to an array

            System.Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
            bool hitWall = false;
            Tracer(mouseTracker.transform.position + mouseTracker.transform.up * SuperOffset, superTracer);
            foreach (RaycastHit hit in hits) { //look at every hit
                if (hit.transform.gameObject.layer == 6) hitWall = true;
                if (!hitWall){ //Check if we hit a wall to prevent going through
                    EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                    if (enemyHealth != null){ //If we find enemy health, lower it and then add him to the list
                        enemyHealth.takeDamage(3);

                        EnemiesHit.Add(hit.transform);
                    }
                }
            }
            Transform[] output = new Transform[EnemiesHit.Count]; 
            for (int i = 0;i < output.Length; i++){ // convert list to array
                output[i] = EnemiesHit[i];
            }

            //SuperCooldownDone = false;
           // StartCoroutine(SuperCooldown());
            return output;
        }

        return null; 
    }

    public IEnumerator SuperCooldown(){
        yield return new WaitForSeconds(SuperShotCooldown);

        SuperCooldownDone = true;
    }

    void Tracer(Vector3 lastHit, GameObject bullet){
        
        GameObject cloned = Instantiate(bullet);
        cloned.transform.position = shotPoint.transform.position;
        cloned.SetActive(true);


        StartCoroutine(TracerHelper(cloned, shotPoint.transform.position, lastHit, 0));

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

    float refVelocity = 0.0f;

    void TurnBody(Transform start){

        float Angle = Mathf.SmoothDampAngle(body.transform.eulerAngles.y, shotPoint.transform.eulerAngles.y, ref refVelocity, TurnUpdateRate);
        body.transform.rotation = Quaternion.Euler(0, Angle, 0);
    }
}
