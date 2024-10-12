using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvade : MonoBehaviour, EnemyMove
{
    public float Cooldown{
        get; set;
    }
    public bool Used{get; set;}
   public float cooldownAccessor;

   public void Start(){
    Cooldown = cooldownAccessor;
   }
   public bool Act(){
        Debug.Log("Evaded");
        return true;
   }
}
