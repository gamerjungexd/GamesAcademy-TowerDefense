using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int startResources = 10;
    private int resources = 10;
    public int Resources { get => this.resources; }

    [SerializeField] private GameObject gameOver = null;
    [SerializeField] private TMP_Text textfieldHealth = null;
    [SerializeField] private TMP_Text textfieldResources = null;

    private int health = 0;
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
