using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    bool clone = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(end());
    }
    public void CloneBullet(){
        clone = true;
        transform.GetChild(0).GetComponent<Animator>().Play("ProjectileForward");
    }

    public IEnumerator end(){
        
        yield return new WaitForSeconds(3f);
        if (clone == true)
        Destroy(this.gameObject);

    }
}
