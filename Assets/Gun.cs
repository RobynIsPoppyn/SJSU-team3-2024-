using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public MovementAndAiming maa; 
    public Battery bat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            print("Fired");
            Fire();
        }
    }

    Transform[] Fire(){ //returns the gameObjects that were hit 

        bat.incrementCharge(-1); //Drain battery
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit[] hits = Physics.RaycastAll(ray, maa.maxDistance, 3); //everything hit in the path
        List<Transform> EnemiesHit = new List<Transform>(); //List of specifically enemies hit, will be coverted to an array

        bool hitWall = false;
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
}
