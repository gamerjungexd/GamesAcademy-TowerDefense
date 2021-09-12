using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;

    [Header("UI:")]
    [SerializeField] private Slider healthbar = null;

    private int health = 0;

    private List<Turret> attackers = new List<Turret>();
    private WaveManager waveManager = null;
    void Awake()
    {
        health = maxHealth;
    }

    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    public void OnDecreaseHealth(int value)
    {

        health -= value;
        if (health <= 0)
        {
            OnDeath();
            return;
        }
        UpdateHealthbar();
    }

    private void UpdateHealthbar()
    {
        if (!healthbar.IsActive())
        {
            healthbar.gameObject.SetActive(true);
        }

        healthbar.value = (1f / (float)maxHealth) * (float)health;
    }

    private void OnDeath()
    {
        waveManager.DecreaseUnitCount();
        foreach (Turret turret in attackers)
        {
            turret.RemoveTarget(gameObject);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Turret turret = other.gameObject.GetComponent<Turret>();
        if (turret != null)
        {
            attackers.Add(turret);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Turret turret = other.gameObject.GetComponent<Turret>();
        if (turret != null)
        {
            attackers.Remove(turret);
        }
    }
}
