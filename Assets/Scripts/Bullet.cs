using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    bool clone = false;
    public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GameObject.Find("EnemyBulletNoise").transform.GetComponent<AudioSource>();
        StartCoroutine(end());
    }
    public void CloneBullet(){
        clone = true;
        transform.GetChild(0).GetComponent<Animator>().Play("ProjectileForward");
    }

    public IEnumerator end(){
        sound.Play();
        
        yield return new WaitForSeconds(3f);
        if (clone == true)
        Destroy(this.gameObject);

    }
}
