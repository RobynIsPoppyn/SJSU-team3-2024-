using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
    public Slider slider;

    public void setHealth(int health)
    {
        slider.value = health;
        Debug.Log("healthSET");
    }

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        Debug.Log("maxHEALTH set");

    }
}
