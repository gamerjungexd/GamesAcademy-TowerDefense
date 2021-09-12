using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float toleranceDistance = 0.1f;

    private int waypointIndex = 0;
    private Transform nextPosition = null;

    private CharacterController characterController = null;
    private WaveManager waveManager = null;

    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        characterController.detectCollisions = false;
    }

    void FixedUpdate()
    {
        if (((Vector2)nextPosition.position - (Vector2)transform.position).magnitude <= toleranceDistance)
        {
            if (waveManager.NextPosition(waypointIndex++, out Transform newPosition))
            {
                waveManager.DecreaseUnitCount();
                Destroy(gameObject);
                return;
            }
            transform.rotation = nextPosition.rotation;
            nextPosition = newPosition;
        }

        Vector2 direction = ((Vector2)nextPosition.position - (Vector2)transform.position).normalized * Time.deltaTime;
        characterController.Move(direction);
    }

    public void InitalizeUnit(WaveManager waveManager, Transform nextPosition)
    {
        this.waveManager = waveManager;
        this.nextPosition = nextPosition;
    }
}
