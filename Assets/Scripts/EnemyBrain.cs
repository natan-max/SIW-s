using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public float Duration = 30f;
    [Range(1, 10)] public float Distance = 1.0f;
    public Enemy[] Enemies;
    public EnemyModelResolver Resolver;
    private float StartTime;

    public bool IsEnemyOnPoint(WaypointList point)
    {
        foreach (Enemy enemy in Enemies)
        {
            if (enemy.CurrentPoint == point)
                return true; 
        }
        return false;
    }

    public void Update()
    {
        if (Time.time - StartTime > Duration)
        {
            StartTime = Time.time;
            DoLogic();
        }
    }

    public void DoLogic()
    {
        foreach (Enemy item in Enemies)
        {
            DoEnemyLogic(item);
        }
    }

    public void DoEnemyLogic(Enemy enemy)
    {
        WaypointList[] MoveOptions = GetMoveOptions(enemy.CurrentPoint);
        WaypointList NeighbourPoint = MoveOptions[Random.Range(0, MoveOptions.Length)];
        GameObject model = Resolver.GetModel(NeighbourPoint, enemy.Id);
        
        if (model == null || IsEnemyOnPoint(NeighbourPoint))
        {
            return;
        }
        
        enemy.CurrentPoint = NeighbourPoint;
        Vector3 point = NeighbourPoint.transform.position;
        enemy.Move(point);
        enemy.SetModel(model);
    }

    public WaypointList[] GetMoveOptions(WaypointList CurrentPoint)
    {
        return CurrentPoint.Waypoints;
    }

}