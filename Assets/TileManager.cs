using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public GameObject[] tiles; // Assign the 8 tiles in the inspector
    public Transform player; // Assign the player transform
    public float tileLength = 30f; // Length of each tile
    public int initialTileCount = 3; // Number of tiles active at start
    private Queue<GameObject> activeTiles = new Queue<GameObject>(); // Active tiles
    private Queue<GameObject> inactiveTiles = new Queue<GameObject>(); // Inactive tiles
    private int loopcount;
    void Start()
    {
        InitializeTiles();
        loopcount=1;
    }

    void Update()
    {
        CheckAndUpdateTiles();
    }

    void InitializeTiles()
    {
        // Add all tiles to the inactive pool
        foreach (GameObject tile in tiles)
        {
            tile.SetActive(false);
            inactiveTiles.Enqueue(tile);
        }

        // Activate only initial tiles
        for (int i = 0; i < initialTileCount; i++)
        {
            ActivateTile(i * tileLength);
        }
    }

    void CheckAndUpdateTiles()
    {
        if (player.position.z >  tileLength*loopcount)
        {
            float lastTileZ = activeTiles.ToArray()[activeTiles.Count - 1].transform.position.z;
            // Deactivate the oldest tile
            GameObject oldTile = activeTiles.Dequeue();
            oldTile.SetActive(false);
            inactiveTiles.Enqueue(oldTile);
            ActivateTile(lastTileZ+tileLength);
            loopcount++;
        }
    }

    void ActivateTile(float zPosition)
    {
        if (inactiveTiles.Count == 0) return; // Safety check

        GameObject newTile = inactiveTiles.Dequeue();
        newTile.SetActive(true);
        newTile.transform.position = new Vector3(0, 0, zPosition);
        activeTiles.Enqueue(newTile);
    }
}
