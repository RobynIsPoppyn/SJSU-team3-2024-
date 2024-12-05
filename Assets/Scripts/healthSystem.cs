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
    
    private Parry parryScript;


    // Start is called before the first frame update
    void Start()
    {
        maa = transform.GetComponent<MovementAndAiming>();
        hb = FindObjectOfType<healthBar>();

        playerHealth = maxHealth;
        hb.setMaxHealth(maxHealth);
        hb.setHealth(playerHealth);

        parryScript = GetComponent<Parry>();
        if (parryScript == null)
        {
            Debug.LogError("Parry script not found on the player object!");
        }
    }

    public void takeDamage(int harm)
    {
        print(parryScript.isInvincible);
        if (parryScript.isInvincible && parryScript.Spinning)
        {
            Debug.Log("Player is invincible! No damage taken.");
            return;
        }
        
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
