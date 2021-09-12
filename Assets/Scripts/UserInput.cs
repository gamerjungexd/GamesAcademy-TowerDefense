using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UserInput : MonoBehaviour
{
    private Tilemap map = null;
    [SerializeField] private Sprite[] TowerSpots = null;

    [SerializeField] private GameObject mouseCanvas = null;
    [SerializeField] private GameObject emptySpot = null;
    [SerializeField] private GameObject turretSpot = null;

    [SerializeField] private Animator animator = null;
    void Start()
    {
        map = FindObjectOfType<Tilemap>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = map.WorldToCell(pos);
            Tile tile = map.GetTile<Tile>(cellPos);
            bool hit = false;
            foreach (Sprite sprite in TowerSpots)
            {
                if (tile.sprite == sprite)
                {
                    hit = true;
                    mouseCanvas.transform.position = map.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0f);
                    animator.SetTrigger("Open");
                    break;
                }
            }

            if(!hit && emptySpot.activeSelf)
            {
                animator.SetTrigger("Close");
            }
        }
    }
}
