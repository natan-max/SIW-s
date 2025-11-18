using System;
using UnityEngine;

public class Laptop : MonoBehaviour, IInteractable
{
    private CamerasScreen _camerasScreen;

    private void Awake()
    {
        _camerasScreen = FindObjectOfType<CamerasScreen>();
    }

    public void Interact()
    {
        _camerasScreen.OpenScreen();
    }
}