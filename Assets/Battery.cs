using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private int Charge;
    public int maxCharge;
    // Start is called before the first frame update
    void Start()
    {
        Charge = maxCharge;
    }

    public void setCharge(int input){
        Charge = input;
    }

    public int getCharge() {
        return Charge;
    }
    

    public void incrementCharge(int input){
        Charge += input; 
    }
}
