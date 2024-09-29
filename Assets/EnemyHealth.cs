using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int health;
    public int maxHealth;

    public void Start(){
        health = maxHealth;
    }
    public void incrementHealth(int input){
        health += input;
    }
    public int getHealth(){
        return health;
    }
}
