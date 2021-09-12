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

    [SerializeField] private Animator animator = null;

    private Vector3 lastClickedCell = Vector3.zero;
    public Vector3 LastClickedCell { get => this.lastClickedCell; }

    private List<GameObject> turrets = new List<GameObject>();
    private Tilemap map = null;

    void Start()
    {
        map = FindObjectOfType<Tilemap>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !emptySpot.activeSelf && !turretSpot.activeSelf)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = map.WorldToCell(pos);
            Vector3 worldPos = map.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0f);

            foreach (GameObject obj in turrets)
            {
                if (obj.transform.position == worldPos)
                {
                    return;
                }
            }

            Tile tile = map.GetTile<Tile>(cellPos);
            bool hit = false;
            foreach (Sprite sprite in TowerSpots)
            {
                if (tile.sprite == sprite)
                {
                    hit = true;
                    mouseCanvas.transform.position = worldPos;
                    animator.SetTrigger("Open");
                    break;
                }
            }

            if (!hit && emptySpot.activeSelf)
            {
                animator.SetTrigger("Close");
            }

            lastClickedCell = worldPos;
        }
        else if (Input.GetMouseButtonDown(0) && emptySpot.activeSelf)
        {
            animator.SetTrigger("Close");
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
