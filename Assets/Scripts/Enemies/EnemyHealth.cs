using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int enemyHealth;

    public enemyHealthBar hb;
    private AudioSource audio;
    public bool deadFlag = false;
    private EnemyAI ea;

    private AudioSource deathAudio; 


    // Start is called before the first frame update
    void Start()
    {   
        audio = GameObject.Find("DamageSound").GetComponent<AudioSource>();
        deathAudio = GameObject.Find("EnemyDeathSound").GetComponent<AudioSource>();
        //hb = transform.GetComponent<enemyHealthBar>(); 
        enemyHealth = maxHealth;
        ea = GetComponent<EnemyAI>();

        
    }
    void Update(){
        hb.updateEnemyHealthBar(enemyHealth,maxHealth);
    }

    public void takeDamage(int harm)
    {
        audio.Play();
        ea.lookRadius = 100f;
        hb.updateEnemyHealthBar(enemyHealth,maxHealth);
      
        enemyHealth -= harm;

        if (enemyHealth <= 0)
        {
            deathAudio.Play();
            deadFlag = true;
            DestroyImmediate(gameObject);
            enemyHealth = 0;
        }
    }
}
