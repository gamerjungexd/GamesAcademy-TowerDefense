using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UserInput : MonoBehaviour
{
    [SerializeField] private Sprite[] TowerSpots = null;

    [SerializeField] private GameObject mouseCanvas = null;
    [SerializeField] private GameObject emptySpot = null;
    [SerializeField] private GameObject turretSpot = null;
    [SerializeField] private GameObject maxSpot = null;

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
        if (Input.GetMouseButtonDown(0) && !emptySpot.activeSelf && !turretSpot.activeSelf && !maxSpot.activeSelf)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = map.WorldToCell(pos);
            Vector3 worldPos = map.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0f);

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
                foreach (Sprite sprite in TowerSpots)
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
        else if (Input.GetMouseButtonDown(0) && emptySpot.activeSelf)
        {
            animator.SetTrigger("Close");
        }
        else if (Input.GetMouseButtonDown(0) && turretSpot.activeSelf)
        {
            animator.SetTrigger("CloseUpgrade");
        }
        else if (Input.GetMouseButtonDown(0) && maxSpot.activeSelf)
        {
            animator.SetTrigger("CloseMax");
        }
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
