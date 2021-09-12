using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 1)]
public class Wave : ScriptableObject
{
    public string waveName = "Wave 00";
    public WaveContent[] content = null;
}

[System.Serializable]
public struct WaveContent
{
    public GameObject unitType;
    public int count;
    public float delay;
    public float delayNextWaveContent;
}
