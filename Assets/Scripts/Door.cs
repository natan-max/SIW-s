using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    public bool IsOpen;
    public Animator DoorAnimator;
    public bool PlayerInZone;

    [ContextMenu ("Interact")]
    public void Interact()
    {
        IsOpen = !IsOpen;
    }
    public void SyncState()
    {
         DoorAnimator.SetBool("IsOpen", IsOpen);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player has entered the trigger zone of " + gameObject.name);
            PlayerInZone = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is in door zone " + gameObject.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is not currently in zone  " + gameObject.name);
            PlayerInZone = false;
        }
    
    }

    

    public void Update()
    {
        
        
    if(Input.GetKeyDown(KeyCode.E) && PlayerInZone == true)
        {
            Debug.Log("key");
        
        }
    }
    private void Start()
    {
        SyncState();
    }
}
