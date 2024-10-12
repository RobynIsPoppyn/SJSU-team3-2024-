using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyMove
{
    public bool Act();
    public float Cooldown{
        get;
        set;
    }
    public bool Used{
        get;
        set;
    }
    public void Start();

}
