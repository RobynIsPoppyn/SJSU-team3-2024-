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
        hb = FindObjectOfType<enemyHealthBar>();

        playerHealth = maxHealth;
        hb.setMaxHealth(maxHealth);
        hb.setHealth(playerHealth);
    }

    public void takeDamage(int harm)
    {
        Debug.Log("OUCH");
        playerHealth -= harm;

        if (playerHealth <= 0)
        {
            deadFlag = true;
            playerHealth = 0;
        }

        hb.setHealth(playerHealth);

    }
}
