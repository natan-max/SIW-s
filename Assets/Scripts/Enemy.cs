using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public WaypointList CurrentPoint;
    public WaypointList StartPoint;
    public EnemyId Id;
    public GameObject Model;
    public GameObject gameOverScreen; 
    public Player player;

    public Transform loseTriggerPoint; 
    private float loseTimer = 0f;
    public float maxLoseTime = 4f;
    private float lightTimer = 0f;
    public float scareTime = 6f;
    private bool isInThreshold = false; 
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
                Debug.Log(name + "Спугнули ворога!");
            }
        }
        else
        {

            lightTimer = 0f;
        }

        if (isInThreshold && !isBeingLit && player != null)
        {
            loseTimer += Time.deltaTime;
            if (loseTimer >= maxLoseTime)
            {
                player.ShowLOX(); 
                loseTimer = 0f;
            }
        }
        else
        {
            loseTimer = 0f;
        }



        isBeingLit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LoseZone")) 
            isInThreshold = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LoseZone"))
        {
            isInThreshold = false;
            loseTimer = 0f; 
        }
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

    public void SetModel(GameObject model)
    {
        Destroy(Model);
        Model = Instantiate(model, transform);
    }

}


