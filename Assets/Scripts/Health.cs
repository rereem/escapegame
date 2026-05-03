using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float DamageAmount;

    public static Health instance ; 
    public HealthBar healthBar;

    private void Awake()
    {
        instance = this;
        
    }

    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void DealDamage()
    {
        currentHealth -= DamageAmount;
        UIcontroller.Instance.ShowDamage();
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            DeathScreen.instance.ShowDeathScreen();
            gameObject.SetActive(false);
        }
    }

}