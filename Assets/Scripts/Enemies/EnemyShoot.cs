using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, EnemyMove
{

    public int defaultDamage = 1;
    public GameObject bulletPrefab;
    private EnemyAI ai; 
    public float cooldownAccessor;
    public float Cooldown{
        get;
        set;
    }
    public bool Used{get;set;}

    public void Start(){
        Cooldown = cooldownAccessor;
        Debug.Log(Cooldown);
    }
    public bool Act(){
        Debug.Log("Shooting");
        GameObject cloned = Instantiate(bulletPrefab, transform.position, 
            transform.rotation, null);
        cloned.GetComponent<Bullet>().CloneBullet();

        
        return true;
    }

    

   

/*
    public Bool Shoot(int Damage){
        GameObject cloned = Instantiate(bulletPrefab, transform.position, 
            transform.rotation, null);
        cloned.GetComponent<Bullet>().CloneBullet();

        return true;
    }
    

    public EnemyMoveList EnemyMove[]{
        {new private class Shoot implements EnemyMove{
            public bool act(){
                GameObject cloned = Instantiate(bulletPrefab, transform.position, 
                transform.rotation, null);
                cloned.GetComponent<Bullet>().CloneBullet();

                return true;
            }
        }
        }
    }
    */
    
}
