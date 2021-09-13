using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private int damageToPlayer = 1;
    [SerializeField] private float toleranceDistance = 0.1f;

    [Header("Model:")]
    [SerializeField] private GameObject[] body = null;

    private int waypointIndex = 0;
    private Vector3 nextPosition = Vector3.zero;
    private Quaternion nextRotation = Quaternion.identity;

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
        if (((Vector2)nextPosition - (Vector2)transform.position).magnitude <= toleranceDistance)
        {
            waypointIndex++;
            if (waveManager.NextPosition(waypointIndex, gameObject.layer, out Vector3 newPosition, out Quaternion newRotation))
            {
                waveManager.DecreaseUnitCount();
                player.OnDecreaseHealth(damageToPlayer);
                Destroy(gameObject);
                return;
            }
            foreach (GameObject obj in body)
            {
                obj.transform.rotation = nextRotation;
            }
            nextPosition = newPosition;
            nextRotation = newRotation;
        }

        Vector2 direction = ((Vector2)nextPosition - (Vector2)transform.position).normalized * Time.deltaTime;
        characterController.Move(direction);
    }

    public void InitalizeUnit(WaveManager waveManager, Player player, Vector3 nextPosition, Quaternion nextRotation, Quaternion rotation)
    {
        this.waveManager = waveManager;
        this.player = player;
        this.nextPosition = nextPosition;
        this.nextRotation = nextRotation;
        foreach (GameObject obj in body)
        {
            obj.transform.rotation = rotation;
        }
    }
}
