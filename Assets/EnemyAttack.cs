using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public int defaultDamage = 1;
    public GameObject bulletPrefab;
    private EnemyAI ai; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool Shoot(int Damage){
        GameObject cloned = Instantiate(bulletPrefab, transform.position, 
            transform.rotation, null);
        cloned.GetComponent<Bullet>().CloneBullet();

        return true;
    }
}
