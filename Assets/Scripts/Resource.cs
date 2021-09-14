using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [Tooltip("The valueSteps after which the next sprite get used as model.\n[Min 1]")]
    [Min(1)]
    [SerializeField] private int valueSteps = 10;
    public int ValueSteps { get => this.valueSteps; }

    [Tooltip("The sprites to use as different values.")]
    [SerializeField] private Sprite[] sprites = null;
    public int maxValueSteps { get => this.sprites.Length; }
    
    [Header("Movement:")]
    [Tooltip("Time until the objects starts to move to the collect point.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] private float delay = 2f;

    [Tooltip("The value how fast the object should move.\n[Min 0.1f]")]
    [Min(0.1f)]
    [SerializeField] private float speed = 5f;

    [Tooltip("The distance the target is reached.\n[Min 0f]")]
    [Min(0f)]
    [SerializeField] private float toleranceDistance = 0.1f;

    [Header("VFX:")]
    [Tooltip("The particleSystem to trigger play.")]
    [SerializeField] private ParticleSystem particle = null;

    private bool startCollect = false;
    private int value = 0;

    private GameObject target = null;
    private SpriteRenderer spriteRenderer = null;
    private Rigidbody2D body = null;
    private Player player = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
    }
    public void InitalizeResource(int initValue, Vector2 force, GameObject target, Player player)
    {
        this.target = target;
        this.player = player;
        this.value = initValue;

        int i;
        for (i = 0; i < sprites.Length; i++)
        {
            initValue = initValue / valueSteps;
            if (initValue < 1)
            {
                break;
            }
        }
        spriteRenderer.sprite = sprites[i];

        body.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(WaitForCollect());
    }

    private IEnumerator WaitForCollect()
    {
        yield return new WaitForSeconds(delay);
        startCollect = true;

        body.velocity = Vector2.zero;
        particle.Play();
    }

    private void Update()
    {
        if (startCollect)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction *= Time.deltaTime * speed;

            transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, direction, Vector3.forward));
            transform.position += direction;

            if (((Vector2)target.transform.position - (Vector2)transform.position).magnitude <= toleranceDistance)
            {
                player.EditResources(value);
                Destroy(gameObject);
            }
        }
    }

}
