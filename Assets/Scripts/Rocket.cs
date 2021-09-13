using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject vfxImpact = null;
    [SerializeField] private float delayLayer = 0.4f;

    [SerializeField] private float toleranceDistance = 0.2f;
    [SerializeField] private ParticleSystem particles = null;

    private bool started = false;
    private Vector3 lastPosition = Vector3.zero;
    private GameObject target = null;
    private int damage = 0;
    private float rocketSpeed = 1f;

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

        Instantiate<GameObject>(vfxImpact, transform.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)), Quaternion.Euler(0, 0, Random.Range(0, 360)));

        Destroy(gameObject);
    }

    private IEnumerator IncreaseLayer()
    {
        yield return new WaitForSeconds(delayLayer);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 8;
        particles.GetComponent<Renderer>().sortingOrder = 7;
    }
}
