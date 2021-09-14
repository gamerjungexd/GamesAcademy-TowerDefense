using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTurret : Turret
{
    [Header("RocketTurret:")]
    [Tooltip("Time to reload the rocket.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] private float reloadTime = 3f;

    //Rocket speed auf dem Turret, weil der Damage ja vom Turret bestimmt wird. Andere Turrets schießen vielleicht langsamere Rockets etc.
    [Tooltip("Speed of the rocket.\n[Min 0.1f]")]
    [Min(0.1f)]
    [SerializeField] private float rocketSpeed = 1f;

    [Space(10f)]
    [Tooltip("The spawner of the Turret for the rockets.")]
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
