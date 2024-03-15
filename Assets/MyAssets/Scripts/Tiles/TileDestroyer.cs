using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour
{
    public Tilemap tilemap;

    public Vector3 pos;

    public void DestroyTile(Vector3 position)
    {
        // Convert the world position to the cell position on the tilemap
        Vector3Int cellPosition = tilemap.WorldToCell(position);

        // Set the tile at the calculated cell position to null, effectively removing it
        tilemap.SetTile(cellPosition, null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, 0.3f);
    }
}













