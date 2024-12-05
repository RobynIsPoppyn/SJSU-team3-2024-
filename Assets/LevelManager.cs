using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private ArrayList enemies = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
       CountEnemies();
    }
    void CountEnemies(){
        enemies = new ArrayList();
        for (int i = 0; i < transform.childCount; i++){
            if (transform.GetChild(i).tag.Equals("Enemy")){
                enemies.Add(transform.GetChild(i));
            }
        }
    }

    public void LoadNext(){
        CountEnemies();
        if (enemies.Count == 0){
            print("Next");
        }
        else print("Kill the rest");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
