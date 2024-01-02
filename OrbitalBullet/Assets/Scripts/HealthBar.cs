using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void Start()
    {
        //setMaxHealth(100);
    }

    void Update()
    {
        //Los dos métodos funcionan
        //transform.LookAt(Camera.main.transform.position, -Vector3.up);
        transform.forward = Camera.main.transform.forward;
    }
}
