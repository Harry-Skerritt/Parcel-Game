using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DropLocation : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Color locationColour;
    [SerializeField] private Color incorrectColour;
    [SerializeField] private Color hoverColour;
    [SerializeField] private string locationName;
    [SerializeField] private int destinationIdentifier;
    
    [SerializeField] private TextMeshPro locationText;
    private SpriteRenderer locationSprite;

    [Header("Events")] 
    [SerializeField] private UnityEvent onCorrectItemDropped;
    [SerializeField] private UnityEvent onIncorrectItemDropped;
    
    [SerializeField] private string parcelTag = "Parcel";


    private void Awake()
    {
        locationSprite = GetComponent<SpriteRenderer>();
        if (locationSprite == null)
        {
            Debug.LogWarning("No Sprite renderer attached");
            enabled = false;
        }
        
        if(onCorrectItemDropped == null) onCorrectItemDropped = new UnityEvent();
        if(onIncorrectItemDropped == null) onIncorrectItemDropped = new UnityEvent();
        
        setLocationLook();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(parcelTag))
        {
            DraggableObject draggable = other.gameObject.GetComponent<DraggableObject>();
            if (draggable != null)
            {
                Debug.Log(other.gameObject.name + " entered " + gameObject.name + " trigger.");
                draggable.setHoveredDropLocation(this);

                if (locationSprite != null)
                {
                    setLocationColour(hoverColour);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(parcelTag))
        {
            DraggableObject draggable = other.gameObject.GetComponent<DraggableObject>();
            if (draggable != null)
            {
                Debug.Log(other.gameObject.name + " exited " + gameObject.name + " trigger.");
                draggable.clearHoveredDropLocation();

                // Reset visual to original color
                if (locationSprite != null)
                {
                    setLocationColour(locationColour);
                }
            }
        }
    }


    public bool attemptDrop(DraggableObject droppedObject)
    {
        bool isCorrectDrop = false;
        ParcelCustomiser customiser = droppedObject.gameObject.GetComponent<ParcelCustomiser>();

        if (customiser != null)
        {
            if (destinationIdentifier == customiser.getDestination())
            {
                // Other Checks?
                Debug.Log($"Correct item ({droppedObject.gameObject.name}) dropped in {gameObject.name}.");
                onCorrectItemDropped.Invoke(); // Trigger correct item event
                customiser.setArrived(true); // Mark the parcel as arrived
                isCorrectDrop = true;
            }
            else
            {
                Debug.Log($"Incorrect item ({droppedObject.gameObject.name}) dropped in {gameObject.name}.");
                if (locationSprite != null)
                {
                    setLocationColour(incorrectColour);
                }
                onIncorrectItemDropped.Invoke(); // Trigger incorrect item event
                isCorrectDrop = false;
            }
        }
        else
        {
            Debug.LogWarning($"AttemptDrop: DraggableObject {droppedObject.gameObject.name} does not have a ParcelCustomiser component.");
            isCorrectDrop = false;
        }
        
        if (locationSprite != null)
        {
            setLocationColour(locationColour);
        }

        return isCorrectDrop;
    }
    
    void setLocationLook()
    {
       setLocationColour(locationColour);
       locationText.text = locationName;
    }

    void setLocationColour(Color newColour)
    {
        locationSprite.color = newColour;
        locationText.color = newColour;
    }

    void initialiseLocation(string name, Color colour)
    {
        locationColour = colour;
        locationName = name;
        setLocationLook();
    }
}
