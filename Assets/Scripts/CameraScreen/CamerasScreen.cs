using System;
using Cinemachine;
using UnityEngine;

public class CamerasScreen : MonoBehaviour
{
    public CameraButtonView Selected;
    public bool ScreenIsOpen;
    
    public CameraButtonView[] Buttons;
    public CinemachineVirtualCamera[] Cameras;

    private void Awake()
    {
        if (Buttons.Length != Cameras.Length)
        {
            throw new Exception("Arrays size should match");
        }

        for (int i = 0; i < Buttons.Length; i++)
        {
            CameraButtonView view = Buttons[i];
            view.SetCamera(Cameras[i]);
            view.OnInteract += SelectCamera;
        }

        if (Selected == null)
        {
            Selected = Buttons[0];
        }
        
        UpdateState();
    }

    public void SelectCamera(CameraButtonView button)
    {
        if (Selected == button)
            return;

        Selected = button;
        UpdateState();
    }

    [ContextMenu("OpenScreen")]
    public void OpenScreen()
    {
        ScreenIsOpen = true;
        UpdateState();
    }

    [ContextMenu("CloseScreen")]
    public void CloseScreen()
    {
        ScreenIsOpen = false;
        UpdateState();
    }

    private void UpdateState()
    {
        gameObject.SetActive(ScreenIsOpen);
        Cursor.lockState = ScreenIsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = ScreenIsOpen;
        
        foreach (var view in Buttons)
        {
            if (ScreenIsOpen && view == Selected)
            {
                view.Display(true);
                view.VirtualCamera.enabled = true;
            }
            else
            {
                view.Display(false);
                view.VirtualCamera.enabled = false;
            }
        }
    }
    
}