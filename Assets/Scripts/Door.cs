using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    public bool IsOpen;
    public Animator DoorAnimator;

    [ContextMenu ("Interact")]
    public void Interact()
    {
        IsOpen = !IsOpen;
        SyncState();
    }
    public void SyncState()
    {
        DoorAnimator.SetBool("IsOpen", IsOpen);
    }
}