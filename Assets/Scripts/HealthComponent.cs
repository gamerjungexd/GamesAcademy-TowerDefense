using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [Tooltip("MaxHealth of the entity.\n[Min 0]")]
    [Min(0)]
    [SerializeField] private int maxHealth = 10;

    [Tooltip("Value of resource points to drop.\n[Min 0]")]
    [Min(0)]
    [SerializeField] private int dropResources = 1;

    [Tooltip("The force of the impulse to push the dropped resources.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] private float maxForce = 10f;

    [Header("Drop:")]
    [Tooltip("The gameObject to instantiate as resource obejct.")]
    [SerializeField] private GameObject resourceObject = null;

    [Header("UI:")]
    [Tooltip("The slider to visualise the health of the entity.")]
    [SerializeField] private Slider healthbar = null;

    private int health = 0;
    private WaveManager waveManager = null;
    private List<Turret> attackers = new List<Turret>();

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
        if (value > 0)
        {
            health -= value;
            if (health <= 0)
            {
                OnDeath();
                return;
            }
            UpdateHealthbar();
        }
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

        RemoveUnitFromTurretTarget();

        Resource res = resourceObject.GetComponent<Resource>();
        int availableResources = dropResources;
        for (int i = res.maxValueSteps - 1; i >= 0; i--)
        {
            int pow = (int)Mathf.Pow(res.ValueSteps, i);
            int value = (int)(availableResources / pow);
            if (value > 0)
            {
                CreateResource(value, ref availableResources, pow);
            }
        }

        Destroy(gameObject);
    }

    private void CreateResource(int value, ref int availableResources, int pow)
    {
        for (int j = 0; j < value; j++)
        {
            GameObject obj = Instantiate<GameObject>(resourceObject, transform.position, transform.rotation);
            Resource temp = obj.GetComponent<Resource>();

            availableResources -= pow;
            temp.InitalizeResource(pow, new Vector2(Random.Range(-maxForce, maxForce), Random.Range(-maxForce, maxForce)), waveManager.ResourceTarget, waveManager.Player);
        }
    }

    public void RemoveUnitFromTurretTarget()
    {
        foreach (Turret turret in attackers)
        {
            turret.RemoveTarget(gameObject);
        }
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
