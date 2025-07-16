using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParcelBehaviour : MonoBehaviour
{
    [Header("Ghost")]
    [SerializeField] private float snapDistance = 0.5f;
    [SerializeField] private GameObject ghostOutlinePrefab;
    
    [Header("Wobble Settings")]
    public float wobbleMagnitude = 5f; // How much it wobbles (degrees of rotation)
    public float wobbleSpeed = 10f;    // How fast it wobbles
    public float wobblePositionMagnitude = 0.05f; // How much it moves positionally
    public float wobblePositionSpeed = 15f; // How fast it moves positionally
    
    // Controlling Ghost and Drag
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 initialDragOffset;
    
    private GameObject currentGhostOutline;
    private bool isDragging = false;
    private Collider2D myCollider;
    private Rigidbody2D myRigidbody;
    
    // Drop Zone

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        if (!isDragging)
        {
            isDragging = true;
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            
            // Calc offset
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialDragOffset = transform.position - (Vector3)mousePosition;
            
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, -0.1f);

            // Create the ghost
            if (ghostOutlinePrefab != null)
            {
                currentGhostOutline = Instantiate(ghostOutlinePrefab, spawnPos, Quaternion.identity);
                currentGhostOutline.name = "GhostOutline_" + gameObject.name;
            }

            if (myCollider != null)
            {
                myCollider.enabled = false;
            }

            if (myRigidbody != null)
            {
                myRigidbody.isKinematic = true;
            }
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPosition = (Vector3)mousePosition + initialDragOffset;
            
            // Apply Wobble Rotation
            float wobbleRotation = Mathf.Sin(Time.time * wobbleSpeed) * wobbleMagnitude;
            transform.rotation = originalRotation * Quaternion.Euler(0, 0, wobbleRotation); // Rotate around Z-axis

            // Apply Wobble Position (optional, can be combined with rotation or just used alone)
            float wobbleX = Mathf.PerlinNoise(Time.time * wobblePositionSpeed, 0) * 2 - 1; // -1 to 1 range
            float wobbleY = Mathf.PerlinNoise(0, Time.time * wobblePositionSpeed) * 2 - 1; // -1 to 1 range
            Vector3 wobbleOffset = new Vector3(wobbleX, wobbleY, 0) * wobblePositionMagnitude;

            transform.position = targetPosition + wobbleOffset;
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            Vector3 targetPosition = this.transform.position;
            transform.rotation = originalRotation;

            if (currentGhostOutline != null)
            {
                targetPosition = currentGhostOutline.transform.position;
                Debug.Log("Snapped to Ghost!");
            }
            else
            {
                // Add dropzone code
            }
            
            transform.position = targetPosition;

            if (myCollider != null)
            {
                myCollider.enabled = true;
            }

            if (myRigidbody != null)
            {
                myRigidbody.isKinematic = false;
            }

            if (currentGhostOutline != null)
            {
                Destroy(currentGhostOutline);
                currentGhostOutline = null;
            }
            
        }
    }
}
