using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource musicPlayer; 
    public AudioClip[] audioClips = new AudioClip[3]; 
    static Music mainPlayer = null;
    private int i = 0;
    void Start(){musicPlayer = transform.GetComponent<AudioSource>();
        i = 0;
        if (mainPlayer == null){

            mainPlayer = this; 
            Object.DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (musicPlayer.isPlaying == false && mainPlayer == this){
            
            musicPlayer.Stop();
            print("track changed");
            musicPlayer.clip = audioClips[i];
            musicPlayer.Play();
            i++; 
            if(i > 2) i = 0;
        }
    }
}
