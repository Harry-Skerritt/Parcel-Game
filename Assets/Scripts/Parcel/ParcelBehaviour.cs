using System;
using UnityEngine;

[RequireComponent(typeof(DraggableObject))]
public class ParcelBehaviour : MonoBehaviour
{
    private DraggableObject draggableObject;
    private ParcelCustomiser parcelCustomiser;
    
    [SerializeField] private LayerMask dropLayer;

    void Awake()
    {
        draggableObject = GetComponent<DraggableObject>();
        if (draggableObject == null)
        {
            Debug.LogError("Parcel Controller requires a DraggableObject");
            enabled = false;
        }

        parcelCustomiser = GetComponent<ParcelCustomiser>();
        if (parcelCustomiser == null)
        {
            Debug.LogError("Parcel Controller requires a ParcelCustomiser");
            enabled = false;
        }
    }

    private void OnEnable()
    {
        draggableObject.OnDragStart.AddListener(HandleDragStart);
        draggableObject.OnSnappedToDropLocation.AddListener(HandleSnappedToDropLocation);
    }

    private void OnDisable()
    {
        draggableObject.OnDragStart.RemoveListener(HandleDragStart);
        draggableObject.OnSnappedToDropLocation.RemoveListener(HandleSnappedToDropLocation);
    }

    private void HandleDragStart()
    {
        Debug.Log(gameObject.name + " was picked up!");
        // Other stuff when picked up
    }

    private void HandleSnappedToDropLocation(DraggableObject obj)
    {
        Debug.Log(obj.name + " (a parcel) was placed on a dz");
        Destroy(gameObject);
    }
    
}
