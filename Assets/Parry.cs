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

    private MovementAndAiming maa;
    // Start is called before the first frame update
    void Start()
    {
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
        
        if (Dodging && DodgeTimer <= DodgeLength){
            cc.enabled = false;
            DodgeTimer += Time.deltaTime;
            rb.MovePosition(transform.position + DodgeForwardDirection * DodgeForce * Time.deltaTime);
            print(DodgeForwardDirection + " " + transform.position + DodgeForwardDirection * DodgeForce * Time.deltaTime);
        }
        else if (DodgeTimer >= DodgeLength){
            cc.enabled = true;
            DodgeTimer = 0f;
            Dodging = false;
        }
    

    }

    public void Spin(){
        Debug.Log(renderer.materials[0]);
        renderer.material = Mat2;
        //Hit detection off
        //Slow movement
        //Activate chain to other moves 

        Spinning = true;
        StartCoroutine(SpinWait());
        

    }
    public void Dodge(){
        if (CanSpinAction()){
            UsedSpinAction = true;
            Debug.Log("Dodge");
            Dodging = true;
            print(transform.forward);
            DodgeDirection = transform.position + transform.forward * DodgeForce;
            DodgeForwardDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (DodgeForwardDirection.Equals(new Vector3(0, 0 , 0))){
                DodgeForwardDirection = transform.forward;
            }
           // rb.AddForce(transform.forward * DodgeForce);
            
        }
    }

    private IEnumerator SpinWait(){
        yield return new WaitForSeconds(SpinTime);
        StopSpin();
    }

    public bool StopSpin(){
        if (Spinning){
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
