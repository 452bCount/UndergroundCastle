using UnityEngine;

public class GridSnap : MonoBehaviour
{
    public Grid grid; // Reference to the isometric grid

    public Transform follower; // The transform of the object to snap to the grid

    private void Update()
    {
        if (follower != null)
        {
            // Convert world position to cell position and back to snap to the grid
            follower.position = grid.CellToWorld(grid.WorldToCell(follower.position));
        }
    }
}
