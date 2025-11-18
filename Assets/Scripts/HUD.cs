using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public Canvas Canvas;
    public Player Player;

    private void Awake()
    {
        Canvas = GetComponent<Canvas>();
        Player = FindObjectOfType<Player>();
    }

    public void Enable()
    {
        if (!Canvas || !Player)
            Awake();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Canvas.enabled = true;
        Player.canMove = true;
    }

    public void Disable()
    {
        if (!Canvas || !Player)
            Awake();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Canvas.enabled = false;
        Player.canMove = false;
    }
}
