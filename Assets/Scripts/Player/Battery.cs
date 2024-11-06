using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private int Charge;
    public int maxCharge;
    Parry parry;
    // Start is called before the first frame update
    void Start()
    {
        Charge = maxCharge;
        parry = transform.GetComponent<Parry>();
    }

    public void setCharge(int input){
        if (!parry.Spinning)
            Charge = input;
    }

    public int getCharge() {
        return Charge;
    }
    

    public void incrementCharge(int input){
        if (!parry.Spinning)
            Charge += input; 
    }
}
