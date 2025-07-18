using UnityEngine;
using UnityEngine.Events;

public class DropZone : MonoBehaviour
{
    [Header("Drop Zone Config")] [SerializeField]
    private string expectedItemTag = "";

    [Header("Events")] [SerializeField] private UnityEvent onCorrectItemDropped;
    [SerializeField] private UnityEvent onIncorrectItemDropped;

    [Header("For Feedback")] [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Color originalColour;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColour = spriteRenderer.color;
        }
        
        if (onCorrectItemDropped == null) onCorrectItemDropped = new UnityEvent();
        if (onIncorrectItemDropped == null) onIncorrectItemDropped = new UnityEvent();
    }

    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogWarning("DropZone '" + name + "' is missing a 2D Collider! Please add one.", this);
        }else if (!collider.isTrigger)
        {
            Debug.LogWarning("DropZone '" + name + "'s collider is not set to 'Is Trigger'! Please enable 'Is Trigger'.", this);
        }

        if (gameObject.layer != LayerMask.NameToLayer("DropZone"))
        {
            Debug.LogWarning("DropZone '" + name + "' is not on the 'DropZone' layer. Snapping might not work correctly. Please assign it in the Inspector.", this);
        }
    }

    public void OnObjectPlaced(DraggableObject placedObj)
    {
        Debug.Log("Object '" + placedObj.name + "' placed in Drop Zone: '" + name + "'");

        bool isCorrectType = false;

        if (!string.IsNullOrEmpty(expectedItemTag))
        {
            if (placedObj.CompareTag(expectedItemTag))
            {
                isCorrectType = true;
                Debug.Log($"Correct item '{placedObj.tag}' dropped into '{name}'!");
                onCorrectItemDropped.Invoke(); // Trigger the correct item event
            }
            else
            {
                Debug.LogWarning($"Incorrect item '{placedObj.tag}' dropped into '{name}'. Expected '{expectedItemTag}'.");
                onIncorrectItemDropped.Invoke(); // Trigger the incorrect item event
            }
        } 
        else
        {
            // Assume no tag expected
            isCorrectType = true;
            Debug.Log($"Any item '{placedObj.name}' accepted by '{name}'.");
            onCorrectItemDropped.Invoke();
        }
        
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashColor(isCorrectType ? Color.green : Color.red, 0.2f)); // Green for correct, red for incorrect
        }
        
        // Do somthing when correct or incorrect item is dropped
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DraggableObject draggable = other.GetComponent<DraggableObject>();
        if (draggable != null)
        {
            if (!string.IsNullOrEmpty(expectedItemTag) && draggable.CompareTag(expectedItemTag))
            {
                if (spriteRenderer != null) spriteRenderer.color = Color.cyan; // Highlight correct type in cyan
            }
            else
            {
                if (spriteRenderer != null) spriteRenderer.color = Color.yellow; // Default highlight in yellow
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting object has a DraggableObject component
        if (other.GetComponent<DraggableObject>() != null)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColour; // Reset to original color
            }
        }
    }

    // Coroutine for flashing color
    private System.Collections.IEnumerator FlashColor(Color flashColor, float duration)
    {
        if (spriteRenderer == null) yield break;

        Color startColor = spriteRenderer.color;
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = startColor; 
    }
}
