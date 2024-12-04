using UnityEngine;

public class BulletHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "BulletObject") 
        {
            Debug.Log("Player hit by bullet!");
            
        }
    }
}
