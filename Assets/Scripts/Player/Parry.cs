using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public bool Spinning{get; private set;}
    [Header("Settings")]

    public float SpinTime = 1f;
    public float SpinCooldown = 0.5f;
    public float DodgeForce = 100f;
    public float DodgeLength = 0.6f;
    public int dodgeSelfDamage = 3;
    public float parrySlow = 0.035f;
    private float defaultSpeed;
    public DashChecker dc;

    public float iFrameDuration = 0.5f;

    public bool isInvincible {get; private set;}
    public int originalLayer;
    

    //Internal variables -- the ones set to public can't be seen in the unity inspector 
    private Rigidbody rb; 
    public bool Dodging{get; private set;}
    private Vector3 DodgeDirection; 
    private Vector3 DodgeForwardDirection;
    private CharacterController cc; 
    private Renderer renderer; 
    private bool CanSpin = true;

    private Material Mat1;
    private Material Mat2;
    public bool UsedSpinAction{get; set;}
    
    public Animator animator;
    private CameraFollow camFollow;
    private MovementAndAiming maa;
    private int SoftCancelDodge; //if the player hits a wall, stops them from jittering, but still keeps them locked into a dodge
    private healthSystem hs; 
    private AudioSource DashSound;
    public static long SpinCounter{get; set;}
    
    // Start is called before the first frame update
    void Start()
    {
        hs = transform.GetComponent<healthSystem>();
        SpinCounter = 0;
        camFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        rb = transform.GetComponent<Rigidbody>();
        cc = transform.GetComponent<CharacterController>();
        maa = transform.GetComponent<MovementAndAiming>();
        renderer = maa.child.GetComponent<Renderer>();
        DashSound = GameObject.Find("DashSound").GetComponent<AudioSource>();

    

    }

    // Update is called once per frame

    private float DodgeTimer;
    Vector3 temp;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Spinning && !maa.Dead){
            StartCoroutine(Spin());
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !maa.Dead){
            Dodge();
        }

        //Dodging
        
        if (Dodging && DodgeTimer <= DodgeLength && SoftCancelDodge == 0){
            if (dc.Colliding == false){
                cc.enabled = false;
                DodgeTimer += Time.deltaTime;
                rb.isKinematic = false;
                rb.MovePosition(transform.position + DodgeForwardDirection * DodgeForce); //Moves rigidbody to dodge after turning off the character controller so there's no conflict
            }
            else {
                rb.isKinematic = false;
                
                SoftCancelDodge++;
            }
        }
        else if (Dodging && DodgeTimer <= DodgeLength){
            DodgeTimer += Time.deltaTime;
        }
        else if (DodgeTimer >= DodgeLength){ 
            rb.isKinematic = true;
            cc.enabled = true;
            DodgeTimer = 0f;
           isInvincible = false;
            Dodging = false; //turn cc back on and  end dodging 
        }
    

    }

    public IEnumerator Spin(){
       if (CanSpin){ //Checks if cooldown off
       DashSound.Play();
        CanSpin = false;
        defaultSpeed = maa.PlayerSpeed;
        maa.PlayerSpeed = parrySlow;
        isInvincible = true;
        long temp = SpinCounter; //If these values are the same later, no other move activated StopSpin 
        
        animator.Play("Spin"); //to be changed later to a different animation
        camFollow.SpinZoom();  //Camera Animation
        Spinning = true;

        StartCoroutine(EnableIframes(iFrameDuration));
        

        yield return new WaitForSeconds(SpinTime);
        if (temp == SpinCounter){
             //We check if this spin has been manually turned off via another player move, or if it has been let run
            StopSpin(true);
            StartCoroutine(InitiateSpinCooldown()); 
        }
       }
       else yield return new WaitForSeconds(0f);
        
        

    }
    public IEnumerator InitiateSpinCooldown(){
        yield return new WaitForSeconds(SpinCooldown);
        CanSpin = true;
    }


    
    public bool StopSpin(bool CalledFromSpin){
        if (!CalledFromSpin){ // If it is called from a spin, there is a cooldown that will reset CanSpin from there, otherwise no cooldown
            CanSpin = true;
        }
        if (Spinning){ //In case of where it is called and no spin is already being done
            isInvincible = false;
            camFollow.SpinUnzoom(); //Camera animation reverts
            
            Spinning = false;
            UsedSpinAction = false;
           

            //renderer.material = Mat1; //was a temporary way to show spinning
            maa.PlayerSpeed = defaultSpeed;
            return true;
        }
        else return false;
    }

    public void Dodge(){
        if (CanSpinAction()){
            DashSound.Play();
            hs.takeDamage(dodgeSelfDamage, false);
            isInvincible = true;
            StartCoroutine(EnableIframes(iFrameDuration));
            SoftCancelDodge = 0;
            cc.enabled = false;  
           // StopSpin();
            UsedSpinAction = true;
          
            bool stopspintemp = StopSpin(false);
            //print("Dodge:  " + CanSpinAction() + "  " + stopspintemp);
            if (stopspintemp){
                SpinCounter++; //Let Spin() know that we've already called StopSpin;
                cc.enabled = false;  //Further continue to turn off character controller so there's no conflicts
                
                Dodging = true;
                
                
                DodgeForwardDirection = new Vector3(0, 0, 0);
                string trigger = "Dodge";
                if(Input.GetKey(KeyCode.W)){ 
                    DodgeForwardDirection.z += 1;
                    trigger = "Dodge";
                }
                if(Input.GetKey(KeyCode.S)){
                    DodgeForwardDirection.z -= 1;
                    trigger = "Dodge";
                }
                if(Input.GetKey(KeyCode.A)){
                    DodgeForwardDirection.x -= 1;
                    trigger = "DodgeLeft";
                }
                if(Input.GetKey(KeyCode.D)){
                    DodgeForwardDirection.x += 1;
                    trigger = "DodgeRight";
                }
                camFollow.SetTrigger(trigger);
                if (DodgeForwardDirection.x != 0 && DodgeForwardDirection.z != 0){ //If two are held at the same time they might do a really strong dodge, so its toned down
                    DodgeForwardDirection.x = DodgeForwardDirection.x / 1.333f;
                    DodgeForwardDirection.z = DodgeForwardDirection.z / 1.333f;
                }
            }
        }
    }


    public bool StopSpin(){
        
        if (Spinning){
            animator.Play("HatDefaultTemp");
            Spinning = false;
            UsedSpinAction = false;
            isInvincible = false;

            renderer.material = Mat1;

            return true;
        }
        else return false;
    }
    public bool CanSpinAction(){
        if (!UsedSpinAction){
            UsedSpinAction = false;
            return true;
        }
        return false; 
    }

    private IEnumerator EnableIframes(float duration)
    {
        print("iFrames");
        
        //gameObject.layer = LayerMask.NameToLayer("IgnoreEnemy"); 
        yield return new WaitForSeconds(duration);
        //gameObject.layer = originalLayer; 
        
    }
}
