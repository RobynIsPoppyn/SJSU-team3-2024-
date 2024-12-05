using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletHit : MonoBehaviour
{
    public int BaseBulletDamage = 1;
    public float SafetyWindow = 0.2f;
    private healthSystem hs;

    private bool Safety = false;

    public void Start(){hs = transform.GetComponent<healthSystem>();}
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 12) 
        {
            
            hs.takeDamage(BaseBulletDamage, Safety);
            StartCoroutine(CooldownSafety());
            Debug.Log("Player hit by bullet!");
            Destroy(collider.gameObject);
        }
    }

    public IEnumerator CooldownSafety(){
        Safety = true;
        yield return new WaitForSeconds(SafetyWindow);
        Safety = false;
    }
}
