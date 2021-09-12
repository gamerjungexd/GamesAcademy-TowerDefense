using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Turret : MonoBehaviour
{
    [SerializeField] private LayerMask targetUnits = 0;

    [SerializeField] private int damage = 2;
    [SerializeField] private float attackSpeed = 1f;

    [Header("Model:")]
    [SerializeField] private Transform modelHead = null;
    [Space(10f)]
    [SerializeField] private float effectTime = 0.5f;
    [SerializeField] private GameObject[] attackEffect = null;

    private int indexAttackEffect = 0;
    private List<GameObject> targets = new List<GameObject>();

    void Start()
    {
        StartCoroutine(ShotTarget());
    }

    private void Update()
    {
        if (modelHead != null && targets.Count > 0)
        {
            modelHead.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, (targets[0].gameObject.transform.position - modelHead.position), Vector3.forward));
        }
    }
    private IEnumerator ShotTarget()
    {
        yield return new WaitForSeconds(attackSpeed);

        yield return new WaitUntil(() => targets.Count > 0);

        HealthComponent component = targets[0].GetComponent<HealthComponent>();
        StartCoroutine(ShowAttack());
        component.OnDecreaseHealth(damage);

        StartCoroutine(ShotTarget());
    }

    private IEnumerator ShowAttack()
    {
        if (attackEffect.Length <= 0)
        {
            yield break;
        }

        attackEffect[indexAttackEffect].SetActive(true);
        yield return new WaitForSeconds(effectTime);
        attackEffect[indexAttackEffect].SetActive(false);

        if (indexAttackEffect < attackEffect.Length - 1)
        {
            indexAttackEffect++;
        }
        else if (attackEffect.Length > 1 && indexAttackEffect >= attackEffect.Length - 1)
        {
            indexAttackEffect = 0;
        }
    }

    public void RemoveTarget(GameObject obj)
    {
        targets.Remove(obj);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((targetUnits.value & (1 << other.gameObject.layer)) > 0)
        {
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
        }
    }
}
