using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthSystem : MonoBehaviour
{
    public float health;
    public float healthMax;

    public void Damage(float damageAmount, Slider healthBarSlider)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject, 1f);
            gameObject.SetActive(false);
        }

        healthBarSlider.value = health / healthMax;
    }
    // public void Heal(float healAmount)
    // {
    //     health -= healAmount;
    //     if (health > healthMax)
    //         health = healthMax;
    // }
}
