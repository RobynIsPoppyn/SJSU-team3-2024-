using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int playerHealth;

    public enemyHealthBar hb;

    public bool deadFlag = false;


    // Start is called before the first frame update
    void Start()
    { 
        playerHealth = maxHealth;

        hb.updateEnemyHealthBar(playerHealth,maxHealth);
    }

    public void takeDamage(int harm)
    {
        Debug.Log("enemyOUCH");
        playerHealth -= harm;

        if (playerHealth <= 0)
        {
            deadFlag = true;
            DestroyImmediate(gameObject);
            playerHealth = 0;
        }
    }
}
