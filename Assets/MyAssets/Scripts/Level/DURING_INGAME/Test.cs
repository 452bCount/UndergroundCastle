using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // How many Grid Columns based on the LevelStage.cs
    #region Grid Columns
    [Min(0)] public float range;
    public int columns; // Renamed to columns for clarity

    [SerializeField] Vector2 screenSpace;
    public static Vector2 _screenSpace;
    public List<Vector3> GridColumns; // Now tracking columns
    public float vertical;
    public float horizontal;
    public int cColumns; // Renamed to cColumns for clarity
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Create how many Grid Columns
        #region Grid Columns
        cColumns = columns; // Using columns instead of rows

        vertical = (float)Camera.main.orthographicSize;
        horizontal = vertical * (float)Camera.main.aspect;

        range = horizontal * 2; // Using horizontal range for vertical lines

        foreach (var r in GetGridColumns(transform, range, columns)) // Now getting columns
        {
            GridColumns.Add(-r.origin);
        }
        #endregion
    }

    Ray[] GetGridColumns(Transform origin, float range, int count)
    {
        Ray[] rays = new Ray[count];
        float spacing = range / (count - 1);
        float start = -range / 2f;
        for (int i = 0; i < count; i++)
        {
            float currentX = start + i * spacing;
            rays[i].origin = new Vector3(currentX, 0, 0) / 1.5f + origin.position;
            rays[i].direction = -origin.up; // Columns extend vertically, so we use up
        }
        return rays;
    }

    private void OnDrawGizmos()
    {
        if (cColumns > 0)
        {
            foreach (var r in GetGridColumns(transform, range, cColumns)) // Drawing columns
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(new Vector3(r.origin.x, vertical), new Vector3(r.origin.x, -vertical));
            }
        }
    }
}
