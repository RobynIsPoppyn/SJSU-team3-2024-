using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public bool Spinning{get; private set;}
    [Header("Settings")]

    public float SpinTime = 1.5f;
    public float SpinCooldown = 1f;
    public float DodgeForce = 100f;
    public float DodgeLength = 0.6f;

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


    public static long SpinCounter{get; set;}
    
    // Start is called before the first frame update
    void Start()
    {
        SpinCounter = 0;
        camFollow = Camera.main.GetComponent<CameraFollow>();
        rb = transform.GetComponent<Rigidbody>();
        cc = transform.GetComponent<CharacterController>();
        maa = transform.GetComponent<MovementAndAiming>();
        renderer = maa.child.GetComponent<Renderer>();
        Mat1 = new Material(renderer.materials[0]);
        Mat2 = new Material(renderer.materials[1]);

    }

    // Update is called once per frame

    private float DodgeTimer;
    Vector3 temp;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Spinning){
            StartCoroutine(Spin());
        }
        else if (Input.GetKeyDown(KeyCode.Space)){
            Dodge();
        }

        //Dodging
        
        if (Dodging && DodgeTimer <= DodgeLength){
            cc.enabled = false;
            DodgeTimer += Time.deltaTime;
            rb.MovePosition(transform.position + DodgeForwardDirection * DodgeForce); //Moves rigidbody to dodge after turning off the character controller so there's no conflict
        }
        else if (DodgeTimer >= DodgeLength){ 
            cc.enabled = true;
            DodgeTimer = 0f;
            Dodging = false; //turn cc back on and  end dodging 
        }
    

    }

    public IEnumerator Spin(){
       if (CanSpin){ //Checks if cooldown off
        CanSpin = false;
        long temp = SpinCounter; //If these values are the same later, no other move activated StopSpin 
        
        animator.Play("HatTestAnim"); //to be changed later to a different animation
        camFollow.SpinZoom();  //Camera Animation
        Spinning = true;
        yield return new WaitForSeconds(SpinTime);
        if (temp == SpinCounter){ //We check if this spin has been manually turned off via another player move, or if it has been let run
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
        animator.Play("HatDefaultTemp");
            camFollow.SpinUnzoom(); //Camera animation reverts
            
            Spinning = false;
            UsedSpinAction = false;
            Debug.Log("Spin off");

            //renderer.material = Mat1; //was a temporary way to show spinning

            return true;
        }
        else return false;
    }

    public void Dodge(){
        if (CanSpinAction()){
            cc.enabled = false;  
           // StopSpin();
            UsedSpinAction = true;
            Debug.Log("Dodge");
        
            if (StopSpin(false)){
                SpinCounter++; //Let Spin() know that we've already called StopSpin;
                cc.enabled = false;  //Further continue to turn off character controller so there's no conflicts
                
                Dodging = true;
                
                
                DodgeForwardDirection = new Vector3(0, 0, 0);
                
                if(Input.GetKey(KeyCode.W)){ 
                    DodgeForwardDirection.z += 1;
                }
                if(Input.GetKey(KeyCode.S)){
                    DodgeForwardDirection.z -= 1;
                }
                if(Input.GetKey(KeyCode.A)){
                    DodgeForwardDirection.x -= 1;
                }
                if(Input.GetKey(KeyCode.D)){
                    DodgeForwardDirection.x += 1;
                }
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
            Debug.Log("Spin off");

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
}
