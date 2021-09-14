using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Tooltip("The distance the target is reached.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] private float toleranceDistance = 0.2f;

    [Header("Rendering:")]
    [Tooltip("The delay after which the sortingOrder gets increased.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] private float delayLayer = 0.4f;

    [Tooltip("The value the model of the object gets sorted after the delay.\n[Min 0]")]
    [Min(0)]
    [SerializeField] private int sortingOrderModel = 8;

    [Tooltip("The value the vfx gets sorted after the delay.\n[Min 0]")]
    [Min(0)]
    [SerializeField] private int sortingOrderVfx = 7;

    [Header("VFX:")]
    [Tooltip("The particleSystem of the vfx to trigger.")]
    [SerializeField] private ParticleSystem particles = null;

    [Space(10f)]
    [Tooltip("Value of the random offset for the impact.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] private float impactArea = 0.25f;

    [Tooltip("GameObject of the vfxImpact to instantiate.")]
    [SerializeField] private GameObject vfxImpact = null;

    private bool started = false;
    private int damage = 0;
    private float rocketSpeed = 1f;
    private Vector3 lastPosition = Vector3.zero;
    private GameObject target = null;

    void Update()
    {
        if (target != null)
        {
            lastPosition = target.transform.position;
        }

        if (started)
        {
            Vector3 direction = (lastPosition - transform.position).normalized;
            direction *= Time.deltaTime * rocketSpeed;

            transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, direction, Vector3.forward));
            transform.position += direction;

            if (((Vector2)lastPosition - (Vector2)transform.position).magnitude <= toleranceDistance)
            {
                OnDamage();
            }
        }
    }

    public void InitalizeRocket(GameObject target, int damage, float rocketSpeed)
    {
        this.target = target;
        this.damage = damage;
        this.rocketSpeed = rocketSpeed;

        started = true;
        particles.Play();
        StartCoroutine(IncreaseLayer());
    }

    private void OnDamage()
    {
        if (target != null)
        {
            HealthComponent component = target.GetComponent<HealthComponent>();
            component.OnDecreaseHealth(damage);
        }

        Instantiate<GameObject>(vfxImpact, transform.position + new Vector3(Random.Range(-impactArea, impactArea), Random.Range(-impactArea, impactArea)), Quaternion.Euler(0, 0, Random.Range(0, 360)));

        Destroy(gameObject);
    }

    private IEnumerator IncreaseLayer()
    {
        yield return new WaitForSeconds(delayLayer);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrderModel;
        particles.GetComponent<Renderer>().sortingOrder = sortingOrderVfx;
    }
}
