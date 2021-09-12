using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTurret : Turret
{
    [Space(10f)]
    [SerializeField] private float effectTime = 0.5f;
    [SerializeField] private GameObject[] attackEffect = null;

    private int indexAttackEffect = 0;

    public override void OnAttack()
    {
        StartCoroutine(ShowAttack());
        HealthComponent component = targets[0].GetComponent<HealthComponent>();
        component.OnDecreaseHealth(damage);
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
}
