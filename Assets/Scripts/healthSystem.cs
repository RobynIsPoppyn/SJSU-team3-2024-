using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthSystem : MonoBehaviour
{
    public int maxHealth = 10;
    public int playerHealth;

    public healthBar hb;

    public bool deadFlag = false;

    private MovementAndAiming maa;
    


    // Start is called before the first frame update
    void Start()
    {
        maa = transform.GetComponent<MovementAndAiming>();
        hb = FindObjectOfType<healthBar>();

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
            Die();
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

    public void Die(){
        print("Dead");
        maa.Dead = true;
    }
}
