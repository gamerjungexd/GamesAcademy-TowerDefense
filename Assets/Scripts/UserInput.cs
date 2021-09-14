using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UserInput : MonoBehaviour
{
    [Tooltip("Offset for the position of the menu to the cell.")]
    [SerializeField] private Vector2 offsetToCell = new Vector2(0.5f, 0.5f);

    [Tooltip("Sprites of the ground where turrets can be created.")]
    [SerializeField] private Sprite[] turretSpots = null;

    [Tooltip("GameObject of the canvas for the mouse menu.")]
    [SerializeField] private GameObject mouseCanvas = null;

    [Tooltip("GameObject of the emptySpot menu category.")]
    [SerializeField] private GameObject emptySpot = null;

    [Tooltip("GameObject of the turretSpot menu category.")]
    [SerializeField] private GameObject turretSpot = null;

    [Tooltip("GameObject of the maxSpot menu category.")]
    [SerializeField] private GameObject maxSpot = null;

    [Tooltip("Animator of the canvas.")]
    [SerializeField] private Animator animator = null;

    private Vector3 lastClickedCell = Vector3.zero;
    public Vector3 LastClickedCell { get => this.lastClickedCell; }

    private GameObject lastClickedTurret = null;
    public GameObject LastClickedTurret { get => this.lastClickedTurret; }

    private List<GameObject> turrets = new List<GameObject>();
    private Tilemap map = null;
    private WaveManager waveManager = null;

    void Start()
    {
        map = FindObjectOfType<Tilemap>();
        waveManager = FindObjectOfType<WaveManager>();
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (emptySpot.activeSelf)
        {
            animator.SetTrigger("Close");
            return;
        }

        if (turretSpot.activeSelf)
        {
            animator.SetTrigger("CloseUpgrade");
            return;
        }

        if (maxSpot.activeSelf)
        {
            animator.SetTrigger("CloseMax");
            return;
        }

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = map.WorldToCell(pos);
        Vector3 worldPos = map.CellToWorld(cellPos) + new Vector3(offsetToCell.x, offsetToCell.y, 0f);

        bool hitTurret = false;

        foreach (GameObject obj in turrets)
        {
            if (obj.transform.position == worldPos)
            {
                hitTurret = true;
                lastClickedTurret = obj;
                mouseCanvas.transform.position = worldPos;
                Turret turret = obj.GetComponent<Turret>();

                if (turret.TypeLevel < waveManager.HighestTurretLevel[turret.Type])
                {
                    animator.SetTrigger("OpenUpgrade");
                }
                else
                {
                    animator.SetTrigger("OpenMax");
                }
                break;
            }
        }

        if (!hitTurret)
        {
            Tile tile = map.GetTile<Tile>(cellPos);
            foreach (Sprite sprite in turretSpots)
            {
                if (tile.sprite == sprite)
                {
                    mouseCanvas.transform.position = worldPos;
                    animator.SetTrigger("Open");
                    break;
                }
            }
        }
        lastClickedCell = worldPos;

    }

    public void AddTurret(GameObject obj)
    {
        turrets.Add(obj);
    }

    public void RemoveTurret(GameObject obj)
    {
        turrets.Remove(obj);
    }
}
