using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthSystem : MonoBehaviour
{
    public int maxHealth = 10;
    public int playerHealth;

    public MovementAndAiming move;
    public healthBar hb;

    public bool deadFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        move = FindObjectOfType<MovementAndAiming>();
        hb = FindObjectOfType<healthBar>();

        playerHealth = maxHealth;
        hb.setHealth(playerHealth);
        hb.setMaxHealth(maxHealth);
    }

    public void takeDamage(int harm)
    {
        Debug.Log("OUCH");
        playerHealth -= harm;

        if (playerHealth <= 0)
        {
            deadFlag = true;
            move.enabled = false;
            playerHealth = 0;
        }

        hb.setHealth(playerHealth);

    }

    public void heal(int heal)
    {
        playerHealth += heal;

        if (playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
        }
    }
}
