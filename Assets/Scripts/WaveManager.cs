using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Waypoints:")]
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private Transform[] waypoints = null;

    [Header("Wave:")]
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private Wave[] waveList = null;

    [Header("UI:")]
    [SerializeField] private TMP_Text textfieldWaveIndex = null;

    private int unitCount = 0;
    private int waveIndex = 0;

    void Start()
    {
        SetUIWaveIndex();

        StartCoroutine(SpawnWave(waveList[waveIndex]));
    }

    public bool NextPosition(int index, out Transform nextPosition)
    {
        if (index >= waypoints.Length)
        {
            nextPosition = null;
            return true;
        }

        nextPosition = waypoints[index];
        return false;
    }

    private void SpawnUnit(GameObject unitType, Transform spawnPoint)
    {
        GameObject unit = Instantiate<GameObject>(unitType, new Vector3(spawnPoint.position.x, spawnPoint.position.y, unitType.transform.position.z), Quaternion.identity);
        unit.GetComponent<UnitMovement>().InitalizeUnit(this, waypoints[0], spawnPoint.rotation);
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
                SpawnUnit(wave.content[i].unitType, spawnPoint);
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
                Debug.Log("WIN!");
                return;
            }

            SetUIWaveIndex();
            StartCoroutine(SpawnWave(waveList[waveIndex]));
        }
    }

    private void SetUIWaveIndex()
    {
        textfieldWaveIndex.text = "" + (waveIndex + 1);
    }
}
