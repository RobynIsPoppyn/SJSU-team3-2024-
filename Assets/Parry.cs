using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{

    public bool Spinning{get; private set;}
    public float SpinTime = 1.5f;
    public float DodgeForce = 100f;
    public float DodgeLength = 0.6f;

    private Rigidbody rb; 
    public bool Dodging{get; private set;}
    private Vector3 DodgeDirection; 
    private Vector3 DodgeForwardDirection;
    private CharacterController cc; 
    private Renderer renderer; 

    private Material Mat1;
    private Material Mat2;
    public bool UsedSpinAction{get; set;}
    
    public Animator animator;
    private CameraFollow camFollow;

    private MovementAndAiming maa;
    // Start is called before the first frame update
    void Start()
    {
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
            Spin();
        }
        else if (Input.GetKeyDown(KeyCode.Space)){
            Dodge();
        }

        //Dodging
        
        
    

    }
    public void FixedUpdate(){
        if (Dodging && DodgeTimer <= DodgeLength){
            cc.enabled = false;
            DodgeTimer += Time.deltaTime;
            rb.MovePosition(transform.position + DodgeForwardDirection * DodgeForce);
            //print(DodgeForwardDirection + " " + transform.position + DodgeForwardDirection * DodgeForce * Time.deltaTime);
        }
        else if (DodgeTimer >= DodgeLength){
            cc.enabled = true;
            DodgeTimer = 0f;
            Dodging = false;
        }
    }

    public void Spin(){
        Debug.Log(renderer.materials[0]);
        animator.Play("HatTestAnim");
        //Hit detection off
        //Slow movement
        //Activate chain to other moves 
        camFollow.SpinZoom();
        Spinning = true;
        StartCoroutine(SpinWait());
        

    }
    public void Dodge(){
        if (CanSpinAction()){
            cc.enabled = false;  
            UsedSpinAction = true;
            Debug.Log("Dodge");
            Dodging = true;
            
            DodgeDirection = transform.position + transform.forward * DodgeForce;
            DodgeForwardDirection = new Vector3(0, 0, 0);
            //DodgeForwardDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            print(DodgeForwardDirection);

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
            if (DodgeForwardDirection.x != 0 && DodgeForwardDirection.z != 0){
                DodgeForwardDirection.x = DodgeForwardDirection.x / 1.333f;
                DodgeForwardDirection.z = DodgeForwardDirection.z / 1.333f;
            }
     

            /*
            if (DodgeForwardDirection.Equals(new Vector3(0, 0, 0))){
                DodgeForwardDirection = transform.forward;
            }
            if (DodgeForwardDirection.x != 0 && DodgeForwardDirection.z != 0){
                DodgeForwardDirection.x = 0.5f * Mathf.Sign(DodgeForwardDirection.x) * (float) Mathf.Abs((int)DodgeForwardDirection.x);
                DodgeForwardDirection.z = 0.5f * Mathf.Sign(DodgeForwardDirection.z) * (float)Mathf.Abs((int)DodgeForwardDirection.z);
            }
            else {
                DodgeForwardDirection.x = Mathf.Sign(DodgeForwardDirection.x) * (float)Mathf.Abs((int)DodgeForwardDirection.x);
                
                DodgeForwardDirection.z = Mathf.Sign(DodgeForwardDirection.z) * (float)Mathf.Abs((int)DodgeForwardDirection.z);
                
            }
            print(DodgeForwardDirection + " After");*/
           // rb.AddForce(transform.forward * DodgeForce);
            
        }
    }

    private IEnumerator SpinWait(){
        yield return new WaitForSeconds(SpinTime);
        StopSpin();
    }

    public bool StopSpin(){
        if (Spinning){
            camFollow.SpinUnzoom();
            
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
