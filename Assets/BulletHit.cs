using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletHit : MonoBehaviour
{
    public int BaseBulletDamage = 1;
    public float SafetyWindow = 0.2f;
    private AudioSource audio;

    private healthSystem hs;

    public bool Safety {get; private set;}

    public void Start(){hs = transform.GetComponent<healthSystem>();}
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 12) 
        {
            if (!Safety){
                print(collider.transform);
                hs.takeDamage(BaseBulletDamage, true);
                StartCoroutine(CooldownSafety());
            
                Destroy(collider.gameObject);
            }
        }
    }

    public IEnumerator CooldownSafety(){
        Safety = true;
        yield return new WaitForSeconds(SafetyWindow);
        Safety = false;
    }
}
