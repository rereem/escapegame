using UnityEngine;

public class Health : MonoBehaviour
{
    public float startingHealth = 100f;
    public HealthBar healthBar; 

    public float HealthPoints
    {
        get { return _healthPoints; }
        set {
            _healthPoints = Mathf.Clamp(value, 0f, startingHealth);
            healthBar.SetHealth(_healthPoints); 
            if (_healthPoints <= 0f)
            {
                Debug.Log("Player is Dead!");
            }
        }
    }

    [SerializeField]
    private float _healthPoints = 100f;

    void Start()
    {
        HealthPoints = startingHealth;
        healthBar.SetMaxHealth(startingHealth); 
    }
}