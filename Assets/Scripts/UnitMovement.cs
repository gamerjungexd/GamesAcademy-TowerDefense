using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(HealthComponent))]
public class UnitMovement : MonoBehaviour
{
    [Tooltip("Damage to the player when reached the end.\n[Min 0]")]
    [Min(0)]
    [SerializeField] private int damageToPlayer = 1;

    [Tooltip("The distance the target is reached.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] private float toleranceDistance = 0.1f;

    [Header("Model:")]
    [Tooltip("GameObjects of the bodys to rotate.")]
    [SerializeField] private GameObject[] body = null;

    private int waypointIndex = 0;
    private Vector3 nextPosition = Vector3.zero;
    private Quaternion nextRotation = Quaternion.identity;

    private CharacterController characterController = null;
    private WaveManager waveManager = null;
    private Player player = null;
    private HealthComponent healthComponent = null;

    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        healthComponent = gameObject.GetComponent<HealthComponent>();
    }

    void FixedUpdate()
    {
        if (((Vector2)nextPosition - (Vector2)transform.position).magnitude <= toleranceDistance)
        {
            waypointIndex++;
            if (waveManager.NextPosition(waypointIndex, gameObject.layer, out Vector3 newPosition, out Quaternion newRotation))
            {
                waveManager.DecreaseUnitCount();
                healthComponent.RemoveUnitFromTurretTarget();
                if (player != null)
                {
                    player.OnDecreaseHealth(damageToPlayer);
                }
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

        Vector2 direction = ((Vector2)nextPosition - (Vector2)transform.position).normalized * Time.fixedDeltaTime;
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
