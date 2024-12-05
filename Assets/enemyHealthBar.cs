using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject cam;

    public void updateEnemyHealthBar(float currentVal, float maxVal)
    {
        slider.value = currentVal/maxVal;
    }

    public void Update()
    {
        Debug.Log(cam.transform + cam.name);
        transform.rotation = cam.transform.rotation;
    }
}
