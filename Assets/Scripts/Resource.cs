using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = null;
    public int maxValueSteps { get => this.sprites.Length; }

    [SerializeField] private int valueSteps = 10;
    [SerializeField] private float delay = 2f;
    [SerializeField] private float speed = 5f;
    public int ValueSteps { get => this.valueSteps; }

    private int value = 0;

    [SerializeField] private float toleranceDistance = 0.1f;

    private SpriteRenderer spriteRenderer = null;
    private Rigidbody2D body = null;
    private GameObject target = null;
    private Player player = null;

    private bool startCollect = false;

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
    }

    private void Update()
    {
        if (startCollect)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction *= Time.deltaTime * speed;

            transform.position += direction;

            if (((Vector2)target.transform.position - (Vector2)transform.position).magnitude <= toleranceDistance)
            {
                player.EditResources(value);
                Destroy(gameObject);
            }
        }
    }

}
