using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();    
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GunShake(){
        animator.Play("GunShake");
    }
    public void SuperShake(){
        animator.Play("SuperGunShake");
    }
    public void SpinZoom(){
        //animator.SetTrigger("Spinning");
        animator.SetTrigger("ZoomOut");
        animator.SetBool("InSpin", true );
    }
    public void SpinUnzoom(){
        animator.SetBool("InSpin", false);
    }
    public void SetTrigger(string s){
        animator.SetTrigger(s);
    }
}
