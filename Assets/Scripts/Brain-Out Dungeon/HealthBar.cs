using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance;

    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthBar;

    void Awake()
    {
        Instance = this;
    }

    public void SetMaxHealth(int health)
    {
        // Set the max value and the current value of the slider into the max health amount
        healthSlider.maxValue = health;
        healthSlider.value = health;

        // Assign the full gradient into the fill image of the heatlhbar
        healthBar.color = healthGradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        // Set the health when lower than the max health
        healthSlider.value = health;
        // Assign the gradient with the normalized value of the health slider as a parameter into the fill image of the heatlhbar
        healthBar.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }
}
