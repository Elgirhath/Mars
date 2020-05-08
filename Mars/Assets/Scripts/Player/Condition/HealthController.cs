using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
    public int maxHealth;
    public int initHealth;
    
    private float _health;
    public float health {
        get => _health;
        set => _health = value;
    }

    public static HealthController instance;
    private HealthBar healthBar;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start() {
        _health = initHealth;
        healthBar = HealthBar.instance;
    }

    public void ChangeHealth(float diff)
    {
        if (_health + diff < 0)
            _health = 0;
        else if (_health + diff > maxHealth)
        {
            _health = maxHealth;
        }
        else
        {
            health += diff;
        }
        
        healthBar.ChangeHealthBar();
        if (_health <= 0.0f) {
            Debug.Log("You died");
        }
    }
}
