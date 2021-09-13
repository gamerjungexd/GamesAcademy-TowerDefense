using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum UnitLayer { Ground = 6, Air = 7 }


public class WaveManager : MonoBehaviour
{
    [Header("Debug:")]
    [SerializeField] private bool loopWaves = false;

    [Header("Waypoints:")]
    [SerializeField] private Transform[] waypoints = null;

    [SerializeField] private Transform[] waypointsAir = null;
    [SerializeField] private int spawnLineDistance = 5;

    [Header("Wave:")]
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private Wave[] waveList = null;

    [Header("UI:")]
    [SerializeField] private TMP_Text textfieldWaveIndex = null;
    [SerializeField] private GameObject gameWin = null;

    [Header("Turrets:")]
    [SerializeField] private GameObject[] TurretUpgrades = null;

    private Dictionary<TurretType, int> highestTurretLevel = new Dictionary<TurretType, int>();
    public Dictionary<TurretType, int> HighestTurretLevel { get => this.highestTurretLevel; }

    private int unitCount = 0;
    private int waveIndex = 0;

    private Player player = null;
    public Player Player { get => this.player; }

    [SerializeField] private GameObject resourceTarget = null;
    public GameObject ResourceTarget { get => this.resourceTarget; }

    void Start()
    {
        player = FindObjectOfType<Player>();

        SetUIWaveIndex();
        InitHighestUpgradeLevel();

        StartCoroutine(SpawnWave(waveList[waveIndex]));
    }

    public bool NextPosition(int index, int layer, out Vector3 nextPosition, out Quaternion nextRotation)
    {
        switch ((UnitLayer)layer)
        {
            case UnitLayer.Ground:
                if (index >= waypoints.Length)
                {
                    nextPosition = Vector3.zero;
                    nextRotation = Quaternion.identity;
                    return true;
                }

                nextPosition = waypoints[index].position;
                nextRotation = waypoints[index].rotation;
                return false;
            case UnitLayer.Air:
                if (index >= waypointsAir.Length)
                {
                    nextPosition = Vector3.zero;
                    nextRotation = Quaternion.identity;
                    return true;
                }

                nextPosition = waypointsAir[index].position;
                nextRotation = waypointsAir[index].rotation;
                return false;
            default:
                nextPosition = Vector3.zero;
                nextRotation = Quaternion.identity;
                return false;
        }
    }

    private void SpawnUnit(GameObject unitType)
    {
        Transform spawnPoint = null;
        Vector3 nextPoint = Vector3.zero;
        Quaternion nextRotation = Quaternion.identity;

        float offsetY = 0f;
        switch ((UnitLayer)unitType.layer)
        {
            case UnitLayer.Ground:
                spawnPoint = waypoints[0];
                nextPoint = waypoints[1].position;
                nextRotation = waypoints[1].rotation;
                break;
            case UnitLayer.Air:
                spawnPoint = waypointsAir[0];
                nextPoint = waypointsAir[1].position;
                nextRotation = waypointsAir[1].rotation;
                offsetY = Random.Range(-spawnLineDistance, spawnLineDistance);
                nextPoint = new Vector3(nextPoint.x, spawnPoint.position.y + offsetY, nextPoint.z);
                break;
        }
        GameObject unit = Instantiate<GameObject>(unitType, new Vector3(spawnPoint.position.x, spawnPoint.position.y + offsetY, unitType.transform.position.z), Quaternion.identity);
        unit.GetComponent<UnitMovement>().InitalizeUnit(this, player, nextPoint, nextRotation, spawnPoint.rotation);
        unitCount++;
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < wave.content.Length; i++)
        {
            for (int j = 0; j < wave.content[i].count; j++)
            {
                yield return new WaitForSeconds(wave.content[i].delay);
                SpawnUnit(wave.content[i].unitType);
            }
            yield return new WaitForSeconds(wave.content[i].delayNextWaveContent);
        }
        yield break;
    }

    public void DecreaseUnitCount()
    {
        unitCount--;

        if (unitCount <= 0)
        {
            waveIndex++;

            if (waveIndex >= waveList.Length)
            {
                if(loopWaves)
                {
                    waveIndex = 0;
                }
                else
                {
                    Time.timeScale = 0f;
                    gameWin.SetActive(true);
                    return;
                }
            }

            SetUIWaveIndex();
            StartCoroutine(SpawnWave(waveList[waveIndex]));
        }
    }

    private void SetUIWaveIndex()
    {
        textfieldWaveIndex.text = "" + (waveIndex + 1);
    }

    public GameObject GetTurretUpgrade(TurretType type, int value)
    {
        foreach (GameObject upgrades in TurretUpgrades)
        {
            if (upgrades.GetComponent<Turret>().Type == type && upgrades.GetComponent<Turret>().TypeLevel == value)
            {
                return upgrades;
            }
        }

        return null;
    }

    private void InitHighestUpgradeLevel()
    {
        foreach (GameObject upgrades in TurretUpgrades)
        {
            Turret turret = upgrades.GetComponent<Turret>();
            if (!highestTurretLevel.ContainsKey(turret.Type))
            {
                highestTurretLevel.Add(turret.Type, turret.TypeLevel);
            }
            else
            {
                if (highestTurretLevel[turret.Type] < turret.TypeLevel)
                {
                    highestTurretLevel[turret.Type] = turret.TypeLevel;
                }
            }
        }
    }
}
