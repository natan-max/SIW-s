using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraButtonView : MonoBehaviour
{
    public event Action<CameraButtonView> OnInteract;
    
    public CinemachineVirtualCamera VirtualCamera;
    
    public TextMeshProUGUI CameraName;
    public Image Background;
    public Image Outline;

    public Color OutlineNormal;
    public Color OutlineSelected;
    public Color CameraNameNormal;
    public Color CameraNameSelected;
    public Color BakgroundNormal;
    public Color BakgroundSelected;

    public void SetCamera(CinemachineVirtualCamera camera)
    {
        VirtualCamera = camera;
    }

    public void Display(bool selected)
    {
        CameraName.color = selected ? CameraNameSelected : CameraNameNormal;
        Background.color = selected ? BakgroundSelected : BakgroundNormal;
        Outline.color = selected ? OutlineSelected : OutlineNormal;
    }

    public void Interact()
    {
        OnInteract?.Invoke(this);
    }
}
