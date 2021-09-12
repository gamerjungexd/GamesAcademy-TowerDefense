using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;

    [SerializeField] private GameObject gameOver = null;
    [SerializeField] private TMP_Text textfieldHealth = null;

    private int health = 0;
    void Start()
    {
        health = maxHealth;

        UpdateHealthbar();
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
        textfieldHealth.text = "" + health;
    }

    private void OnDeath()
    {
        Time.timeScale = 0f;
        gameOver.SetActive(true);
    }
}
