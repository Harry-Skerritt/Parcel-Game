using UnityEngine;
using UnityEngine.Events;

public class DraggableObject : MonoBehaviour
{
    [Header("Snapping Settings")] 
    [SerializeField] private bool canSnap = true;
    [SerializeField] private GameObject ghostOutlinePrefab;

    [Header("Wobble Settings")] 
    [SerializeField] private float wobbleMagnitude = 5f;
    [SerializeField]  private float wobbleSpeed = 12f;
    [SerializeField]  private float wobblePositionMagnitude = 0.02f;
    [SerializeField] private float wobblePositionSpeed = 15f;

    
    [Header("Drop Zone Settings")]
    
    // Events to notify other components
    [HideInInspector] public UnityEvent OnDragStart = new UnityEvent();
    [HideInInspector] public UnityEvent<Vector3> OnDragEnd = new UnityEvent<Vector3>(); 
    [HideInInspector] public UnityEvent<DraggableObject, GameObject> OnSnappedToGhost = new UnityEvent<DraggableObject, GameObject>();
    [HideInInspector] public UnityEvent<DraggableObject> OnSnappedToDropLocation = new UnityEvent<DraggableObject>();
    
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 initialDragOffset;
    private GameObject currentGhostOutline;
    private bool isDragging = false;
    private Collider2D myCollider;
    private Rigidbody2D myRigidbody;
    
    private DropLocation currentHoveredDropLocation;

    void Awake()
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
            
            if (myRigidbody != null)
            {
                myRigidbody.isKinematic = true;
                myRigidbody.velocity = Vector2.zero;
                myRigidbody.angularVelocity = 0f;
            }

            OnDragStart.Invoke();
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

             if (canSnap)
             {
                 if (currentHoveredDropLocation != null)
                 {
                     bool dropSuccessful = currentHoveredDropLocation.attemptDrop(this);
                     
                     // Look at refactoring this
                     if (dropSuccessful || !dropSuccessful)
                     {
                         targetPosition = currentHoveredDropLocation.transform.position;
                         Debug.Log($"Snapped to DropZone: {currentHoveredDropLocation.gameObject.name}");
                         OnSnappedToDropLocation.Invoke(this);             
                     }
                 }
                 // Check for ghost
                 else if (currentGhostOutline != null)
                 {
                     targetPosition = currentGhostOutline.transform.position;
                     Debug.Log("Snapped to Ghost!");
                     OnSnappedToGhost.Invoke(this, currentGhostOutline);
                 }
             }
            
             transform.position = targetPosition;

             if (myCollider != null)
             {
                 myCollider.enabled = true;
             }

             if (myRigidbody != null && myRigidbody.bodyType == RigidbodyType2D.Dynamic) // Restore dynamic behavior if it was dynamic
             {
                 myRigidbody.isKinematic = false;
             }

             if (currentGhostOutline != null)
             {
                 Destroy(currentGhostOutline);
                 currentGhostOutline = null;
             }
                
             OnDragEnd.Invoke(targetPosition);
             currentHoveredDropLocation = null;
         }
     }

    public void setHoveredDropLocation(DropLocation dropLocation)
    {
        currentHoveredDropLocation = dropLocation;
        Debug.Log($"{gameObject.name} is hovering over {dropLocation.gameObject.name}");
    }
    
    public void clearHoveredDropLocation()
    {
        if (currentHoveredDropLocation != null)
        {
            Debug.Log($"{gameObject.name} is no longer hovering over {currentHoveredDropLocation.gameObject.name}");
        }
        currentHoveredDropLocation = null;
    }
    public bool IsDragging()
    {
        return isDragging;
    }
}
