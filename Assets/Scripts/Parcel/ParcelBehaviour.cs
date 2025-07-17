using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(DraggableObject))]
public class ParcelBehaviour : MonoBehaviour
{
    private DraggableObject draggableObject;

    void Awake()
    {
        draggableObject = GetComponent<DraggableObject>();
        if (draggableObject == null)
        {
            Debug.LogError("Parcel Controller requires a DraggableObject");
            enabled = false;
        }
    }

    private void OnEnable()
    {
        draggableObject.OnDragStart.AddListener(HandleDragStart);
        draggableObject.OnSnappedToDropZone.AddListener(HandleSnappedToDropZone);
    }

    private void OnDisable()
    {
        draggableObject.OnDragStart.RemoveListener(HandleDragStart);
        draggableObject.OnSnappedToDropZone.RemoveListener(HandleSnappedToDropZone);
    }

    private void HandleDragStart()
    {
        Debug.Log(gameObject.name + " was picked up!");
        // Other stuff when picked up
    }

    private void HandleSnappedToDropZone(DraggableObject obj, DropZone zone)
    {
        Debug.Log(obj.name + " (a parcel) was placed in " + zone.name);
        zone.OnObjectPlaced(obj);
    }
}
