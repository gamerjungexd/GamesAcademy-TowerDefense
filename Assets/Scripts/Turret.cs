using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Turret : MonoBehaviour
{
    [Tooltip("Type of the turret.")]
    [SerializeField] private TurretType type = 0;
    public TurretType Type { get => this.type; }

    [Tooltip("Which upgrade level is the turret of his type.\n[Min 0]")]
    [Min(0)]
    [SerializeField] private int typeLevel = 0;
    public int TypeLevel { get => this.typeLevel; }

    [Tooltip("Cost of the turret.\n[Min 0]")]
    [Min(0)]
    [SerializeField] private int cost = 1;
    public int Cost { get => this.cost; }

    [Space(10f)]
    [Tooltip("Which units the turret should target.")]
    [SerializeField] private LayerMask targetUnits = 0;

    [Tooltip("Damage of the turret.\n[Min 0]")]
    [Min(0)]
    [SerializeField] protected int damage = 2;

    [Tooltip("How long should the turret wait between the attacks in seconds.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] protected float attackSpeed = 1f;

    [Header("Model:")]
    [Tooltip("Model of the head to rotate.")]
    [SerializeField] private Transform modelHead = null;

    protected List<GameObject> targets = new List<GameObject>();

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
    public virtual IEnumerator ShotTarget()
    {
        yield return new WaitForSeconds(attackSpeed);

        yield return new WaitUntil(() => targets.Count > 0);

        OnAttack();

        StartCoroutine(ShotTarget());
    }

    public virtual void OnAttack()
    {

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
