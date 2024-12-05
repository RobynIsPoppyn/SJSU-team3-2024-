using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class healthSystem : MonoBehaviour
{
    public int maxHealth = 10;
    public int parryHeal = 3;
    public int playerHealth;

    public healthBar hb;

    public bool deadFlag = false;

    private MovementAndAiming maa;
    
    private Parry parryScript;
    private Animator animPP;
    private BulletHit bh; 


    // Start is called before the first frame update
    void Start()
    {
        bh = transform.GetComponent<BulletHit>();
        maa = transform.GetComponent<MovementAndAiming>();
        hb = FindObjectOfType<healthBar>();
        animPP = GameObject.Find("GlobalPostProcessing").GetComponent<Animator>();
        
        playerHealth = maxHealth;
        hb.setMaxHealth(maxHealth);
        hb.setHealth(playerHealth);

        parryScript = GetComponent<Parry>();
        if (parryScript == null)
        {
            Debug.LogError("Parry script not found on the player object!");
        }
    }

    void Update(){
        hb.setHealth(playerHealth);
    }

    public void takeDamage(int harm)
    {
        takeDamage(harm, parryScript.isInvincible);

    }

    public void takeDamage(int harm, bool Invincible){
        print(Invincible);
        if (Invincible || bh.Safety)
        {
            if (parryScript.Spinning) heal(parryHeal);
            Debug.Log("Player is invincible! No damage taken.");
            return;
        }
        
        
        playerHealth -= harm;
        animPP.Play("Base Layer.DamagePP");
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
        animPP.Play("Base Layer.HealPP");
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

    public void DieHelper(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
