using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public WaypointList CurrentPoint;
    public WaypointList StartPoint;

    private float lightTimer = 0f;        
    public float scareTime = 6f;         
    private bool isBeingLit = false;      
    private bool isScared = false;        

    public void OnHitByLight()
    {
        isBeingLit = true; 
    }

    void Start()
    {
        StartPoint = CurrentPoint;
    }

    void Update()
    {
        if (isScared) return; 

        if (isBeingLit)
        {
            lightTimer += Time.deltaTime;

            if (lightTimer >= scareTime)
            {
                isScared = true;
                KnockEnemyBack();
                Debug.Log(name + " злякався після " + scareTime + " секунд світла!");
            }
        }
        else
        {
            
            lightTimer = 0f;
        }

        isBeingLit = false;
    }

    public void KnockEnemyBack()
    {
        Move(StartPoint);
    }
    
    public void Move(WaypointList Point)
    {
        CurrentPoint = Point;
        Vector3 point = Point.transform.position;
        Move(point);
    }

    public void Move(Vector3 TargetPosition)
    {
        transform.position = TargetPosition;
    }

}


