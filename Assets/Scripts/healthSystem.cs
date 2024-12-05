using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthSystem : MonoBehaviour
{
    public int maxHealth = 10;
    public int playerHealth;

    public healthBar hb;

    public bool deadFlag = false;

    private Parry parryScript;


    // Start is called before the first frame update
    void Start()
    {
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
        if (parryScript != null && parryScript.isInvincible)
        {
            Debug.Log("Player is invincible! No damage taken.");
            return;
        }
        Debug.Log("OUCH");
        playerHealth -= harm;

        if (playerHealth <= 0)
        {
            deadFlag = true;
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
