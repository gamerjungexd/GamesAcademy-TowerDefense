using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [Tooltip("The time until the object get destroyed.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] private float timeUntilDestroy = 5f;
    void Start()
    {
        Destroy(gameObject, timeUntilDestroy);
    }
}
