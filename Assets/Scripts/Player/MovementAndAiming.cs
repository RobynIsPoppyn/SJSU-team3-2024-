using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAndAiming : MonoBehaviour
{
    public Rigidbody rb;
    public CharacterController controller;
    public Animator animator;
    public Camera cam;
    [Header("General Values")]
    public float PlayerSpeed = 50;
    
    public float gravityValue = 0.05f;
    private Parry parry;
    public AudioSource walkSound;
     
    
    private Vector3 playerVelocity;
    public GameObject mouseTracker;
    public LineRenderer lineRenderer;
    public bool Dead {get; set;}
    public bool DyingProcess;


    
    // Start is called before the first frame update
    void Start()
    {
        Dead = false;
        DyingProcess = false;
        playerVelocity = new Vector3(0, 0, 0);
        parry = transform.GetComponent<Parry>();
        mouseTracker = GameObject.Find("MouseTracker");
        walkSound = GameObject.Find("WalkNoise").GetComponent<AudioSource>();

       // mouseTracker = new GameObject(); //Creates an object for the character to look at that follows the mouse
    }

    [Header("Grounded Sphere Check")]
    private bool isGrounded;
    public float groundedSphereOffset = 0.5f;
    public float groundedSphereRadius = 0.1f;

    [Header("Mouse tracking")]
    public float maxDistance = 50f;

    public Transform child; 
    //Draw the grounded sphere
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(
            new Vector3(
                transform.position.x, transform.position.y - groundedSphereOffset, 
                transform.position.z), groundedSphereRadius
        );
    }
    
    Vector3 mousePos;


    void FixedUpdate()
    {
        if (Dead) {
            DeathAnim(DyingProcess);
        }
        
        
        //mousePos = Input.mousePosition;
        //Updates position of grounded sphere location, could be better optimized if issues occur
        Vector3 groundedSphereLocation = new Vector3(transform.position.x, transform.position.y - groundedSphereOffset, transform.position.z);
        isGrounded = Physics.CheckSphere( //Checking if the player is near the ground
            groundedSphereLocation,
            groundedSphereRadius, gameObject.layer);
        

        bool groundedPlayer = controller.isGrounded; //Linearly lower gravity unless player is touching the ground
        if (isGrounded && playerVelocity.y < 0) 
        {
            playerVelocity.y = 0f;
        }
        else if (!isGrounded){
            playerVelocity.y -= gravityValue;
        }


        

        //movement
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (direction.x != 0 && direction.z != 0){
            direction.x = direction.x * 0.85f;
            direction.z = direction.z * 0.85f;
        }
        if (controller.enabled && !Dead){
            controller.Move(direction * PlayerSpeed);
            controller.Move(playerVelocity); //move down
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Walk") && !direction.Equals(Vector3.zero) && !parry.Spinning){
                animator.Play("Walk");
                if(walkSound.isPlaying == false){
                    walkSound.Play();
                }
              // print("walking anim" + animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Walk"));
            }
            else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle") && Vector3.zero.Equals(direction * PlayerSpeed)){
                animator.SetTrigger("Idle");
                walkSound.Stop();
            }
        }
        else if (Dead) {
            walkSound.Stop();
        }




        Vector3 temp = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0); //current position of the mouse
        temp.z = 10; //fix depth issues
        mousePos = Camera.main.ScreenToWorldPoint(temp);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

        RaycastHit hitInfo;

        

        LayerMask mask = LayerMask.GetMask("Player");
        if (Physics.Raycast(ray, out hitInfo, maxDistance, ~(1 << 3 | 1 << 2))){
            mouseTracker.transform.position = hitInfo.point;
        }/*
        if (Input.GetKey(KeyCode.Mouse1)){
            lineRenderer.enabled = true;
            Vector3[] lineEnds = new Vector3[2]{mouseTracker.transform.position, child.transform.position}; //For drawing the laser 
            lineRenderer.SetPositions(lineEnds); //Set laser positions
        }
        else {lineRenderer.enabled = false;}*/
        if (!Dead)
        child.LookAt(mouseTracker.transform, Vector3.up);


    }


    //Screen size will be divided by 100 
    public static int pixelsTo100(int input, bool vertical){ //uses screen width if false, screen height if true
        float scale;
        if (vertical) scale = 100 / Screen.height;
        else scale = 100 / Screen.width;

        return (int) input * (int)scale;
    }

    public Transform mouseTarget(){
        return mouseTracker.transform;
    }
    public void DeathAnim(bool Done){
        if (!Done){
            DyingProcess = true;
            animator.Play("Death 0");
        }
    }

    
}
