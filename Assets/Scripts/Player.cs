using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Tooltip("The maxHealth of the player to start with.\n[Min 0]")]
    [Min(0)]
    [SerializeField] private int maxHealth = 20;

    [Tooltip("The resource points at the begin of the level.")]
    [SerializeField] private int startResources = 10;

    [Header("UI:")]
    [Tooltip("The gameObject to turn on when gameover.")]
    [SerializeField] private GameObject gameOver = null;

    [Tooltip("The TMP_Text object to show the players health.")]
    [SerializeField] private TMP_Text textfieldHealth = null;

    [Tooltip("The TMP_Text object to show the players resources.")]
    [SerializeField] private TMP_Text textfieldResources = null;

    private int health = 0;
    private int resources = 10;
    public int Resources { get => this.resources; }

    void Start()
    {
        health = maxHealth;
        resources = startResources;

        UpdateHealthbar();
        UpdateUIResource();
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

    public void EditResources(int value)
    {
        resources += value;
        UpdateUIResource();
    }

    private void UpdateUIResource()
    {
        textfieldResources.text = "" + resources;
    }
}
