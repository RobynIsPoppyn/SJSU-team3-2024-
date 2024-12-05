using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public int BaseBulletDamage = 1;
    private healthSystem hs;

    public void Start(){hs = transform.GetComponent<healthSystem>();}
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 12) 
        {
            hs.takeDamage(1);
            Debug.Log("Player hit by bullet!");
            
        }
    }
}
