using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int enemyHealth;

    public enemyHealthBar hb;

    public bool deadFlag = false;


    // Start is called before the first frame update
    void Start()
    { 
        hb = GetComponent<enemyHealthBar>(); 
        enemyHealth = maxHealth;

        hb.updateEnemyHealthBar(enemyHealth,maxHealth);
    }

    public void takeDamage(int harm)
    {
        Debug.Log("enemyOUCH");
        enemyHealth -= harm;

        if (enemyHealth <= 0)
        {
            deadFlag = true;
            DestroyImmediate(gameObject);
            enemyHealth = 0;
        }
    }
}
