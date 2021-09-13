using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTurret : Turret
{
    [SerializeField] private float reloadTime = 3f;
    [SerializeField] private float rocketSpeed = 1f;
    [SerializeField] private RocketSpawn[] spawner = null;
    private int indexSpawner = 0;
    public override void OnAttack()
    {
        if (targets.Count > 0)
        {
            spawner[indexSpawner].StartRocket(targets[0], damage, reloadTime, rocketSpeed);
        }
    }

    public override IEnumerator ShotTarget()
    {
        yield return new WaitForSeconds(attackSpeed);

        yield return new WaitUntil(() => targets.Count > 0);

        while (!spawner[indexSpawner].IsReady)
        {
            indexSpawner++;
            if (indexSpawner >= spawner.Length)
            {
                indexSpawner = 0;
            }
            yield return null;
        }

        OnAttack();

        StartCoroutine(ShotTarget());
    }
}
