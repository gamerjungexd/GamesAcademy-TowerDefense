using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHead : MonoBehaviour
{
    [Tooltip("The min and max of the random value to rotate the head.")]
    [SerializeField] private Vector2 rotationAngle = new Vector2(-180f, 180f);

    [Tooltip("The min and max of the random duration to rotate the head.")]
    [SerializeField] private Vector2 rotationDuration = new Vector2(0.5f, 2f);

    [Tooltip("The min and max of the random duration to wait for the next rotation.")]
    [SerializeField] private Vector2 waitForNextDuration = new Vector2(0.5f, 2f);

    [Space(10f)]
    [Tooltip("GameObject of the body model.")]
    [SerializeField] private GameObject body = null;

    [Tooltip("GameObject of the head model.")]
    [SerializeField] private GameObject head = null;

    private float t = 0f;

    private float nextRotationDuration = 1f;
    private float startTime = 0f;
    private Vector3 nextRotation = Vector3.zero;

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
