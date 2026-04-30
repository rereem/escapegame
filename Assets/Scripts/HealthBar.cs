using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI healthText;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.text = health + "/" + health; 
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        healthText.text = Mathf.RoundToInt(health) + "/" + Mathf.RoundToInt(slider.maxValue); 
    }
}