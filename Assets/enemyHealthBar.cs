using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Camera cam;

    public void updateEnemyHealthBar(float currentVal, float maxVal)
    {
        slider.value = currentVal/maxVal;
    }

    public void Update()
    {
        Vector3 targetPosition = new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z);
        transform.LookAt(targetPosition);

    }
}
