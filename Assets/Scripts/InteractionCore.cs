using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class InteractionCore : MonoBehaviour
{
    public Camera Camera;
    public float InteractionDistance = 10;
    public TextMeshProUGUI Hint;
    void Start()
    {
        
    }
    private void ShowHint()
    {
        Hint.enabled = true;
    }


    private void HideHint()
    {
        Hint.enabled = false;
    }




    void Update()
    {
        Ray ray = Camera.ScreenPointToRay(new Vector3(0.5f, 0.5f));
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit);

        if (hitSomething && hit.transform.TryGetComponent(out IInteractable interactableObject))
        {
            ShowHint();
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactableObject.Interact();
            }

        }
        else
        {
            HideHint();
        }

    
    }
}
