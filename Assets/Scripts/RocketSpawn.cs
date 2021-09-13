using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawn : MonoBehaviour
{
    private bool isReady = false;
    public bool IsReady { get => this.isReady; }

    [SerializeField] private GameObject rocketType = null;
    private Rocket rocket = null;

    private void Start()
    {
        rocket = Instantiate<GameObject>(rocketType, transform).GetComponent<Rocket>();
        isReady = true;
    }
    public IEnumerator ReloadRocket(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);

        rocket = Instantiate<GameObject>(rocketType, transform).GetComponent<Rocket>();
        isReady = true;
    }

    public void StartRocket(GameObject target, int damage, float reloadTime, float rocketSpeed)
    {
        isReady = false;
        rocket.transform.SetParent(null);
        rocket.InitalizeRocket(target, damage, rocketSpeed);
        StartCoroutine(ReloadRocket(reloadTime));
    }
}
