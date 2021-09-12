using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private int damageToPlayer = 1;
    [SerializeField] private float toleranceDistance = 0.1f;

    [Header("Model:")]
    [SerializeField] private GameObject body = null;

    private int waypointIndex = 0;
    private Transform nextPosition = null;

    private CharacterController characterController = null;
    private WaveManager waveManager = null;
    private Player player = null;

    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        //characterController.detectCollisions = false;
    }

    void FixedUpdate()
    {
        if (((Vector2)nextPosition.position - (Vector2)transform.position).magnitude <= toleranceDistance)
        {
            if (waveManager.NextPosition(waypointIndex++, out Transform newPosition))
            {
                waveManager.DecreaseUnitCount();
                player.OnDecreaseHealth(damageToPlayer);
                Destroy(gameObject);
                return;
            }
            body.transform.rotation = nextPosition.rotation;
            nextPosition = newPosition;
        }

        Vector2 direction = ((Vector2)nextPosition.position - (Vector2)transform.position).normalized * Time.deltaTime;
        characterController.Move(direction);
    }

    public void InitalizeUnit(WaveManager waveManager, Player player, Transform nextPosition, Quaternion rotation)
    {
        this.waveManager = waveManager;
        this.player = player;
        this.nextPosition = nextPosition;
        body.transform.rotation = rotation;
    }
}
