using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointList : MonoBehaviour
{
    public WaypointList[] Waypoints;
    public bool IsOccupied;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 position = transform.position;
        foreach (var item in Waypoints)
        {
            Vector3 ItemPosition = item.transform.position;
            Gizmos.DrawLine(position, ItemPosition);
        }
    }
}
