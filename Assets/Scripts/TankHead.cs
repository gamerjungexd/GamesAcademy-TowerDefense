using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHead : MonoBehaviour
{
    [SerializeField] private GameObject head = null;
    [SerializeField] private GameObject body = null;

    [SerializeField] private Vector2 rotationAngle = Vector2.zero;
    [SerializeField] private Vector2 rotationDuration = Vector2.zero;
    [SerializeField] private Vector2 waitForNextDuration = Vector2.zero;

    private Vector3 nextRotation = Vector3.zero;
    private float nextRotationDuration = 1f;
    private float startTime = 0f;

    private float t = 0f;

    void Start()
    {
        head.transform.rotation = body.transform.rotation;
        StartCoroutine(RotateHead());
    }

    void Update()
    {
        if (t < 1f)
        {
            t = (Time.time - startTime) / nextRotationDuration;
            head.transform.rotation = Quaternion.Euler(Vector3.Slerp(head.transform.rotation.eulerAngles, nextRotation, t * Time.deltaTime));
        }
    }

    private IEnumerator RotateHead()
    {
        yield return new WaitForSeconds(nextRotationDuration + Random.Range(waitForNextDuration.x, waitForNextDuration.y));

        float next = Random.Range(rotationAngle.x, rotationAngle.y);
        nextRotation = new Vector3(0, 0, head.transform.rotation.eulerAngles.z + next);

        nextRotationDuration = Random.Range(rotationDuration.x, rotationDuration.y);
        t = 0f;

        startTime = Time.time;

        StartCoroutine(RotateHead());
    }
}
