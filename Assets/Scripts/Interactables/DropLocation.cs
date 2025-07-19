using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DropLocation : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] protected Color locationColour;
    [SerializeField] protected Color correctColour;
    [SerializeField] protected Color incorrectColour;
    [SerializeField] protected Color hoverColour;
    [SerializeField] protected string locationName;
    [SerializeField] protected int destinationIdentifier;
    
    [SerializeField] protected TextMeshPro locationText = null;
    protected SpriteRenderer locationSprite;

    [Header("Events")] 
    [SerializeField] protected UnityEvent onCorrectItemDropped;
    [SerializeField] protected UnityEvent onIncorrectItemDropped;
    
    [SerializeField] protected string parcelTag = "Parcel";


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
                correctItem(droppedObject, customiser);
                isCorrectDrop = true;
            }
            else
            {
                incorrectItem(droppedObject, customiser);
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
    
    protected void setLocationLook()
    {
       setLocationColour(locationColour);
       if (locationText != null)
       {
           locationText.text = locationName;
       }
    }

    protected void setLocationColour(Color newColour)
    {
        if (locationSprite != null)
        {
            locationSprite.color = newColour;

        }

        if (locationText != null)
        {
            locationText.color = newColour;

        }
    }

    protected void initialiseLocation(string name, Color colour)
    {
        locationColour = colour;
        locationName = name;
        setLocationLook();
    }

    protected void correctItem(DraggableObject droppedObject, ParcelCustomiser customiser)
    {
        Debug.Log($"Correct item ({droppedObject.gameObject.name}) dropped in {gameObject.name}.");
        if (locationSprite != null)
        {
            setLocationColour(correctColour);
        }
                
        onCorrectItemDropped.Invoke(); // Trigger correct item event
        customiser.setArrived(true); // Mark the parcel as arrived
    }

    protected void incorrectItem(DraggableObject droppedObject, ParcelCustomiser customiser)
    {
        Debug.Log($"Incorrect item ({droppedObject.gameObject.name}) dropped in {gameObject.name}.");
        if (locationSprite != null)
        {
            setLocationColour(incorrectColour);
        }
                
        onIncorrectItemDropped.Invoke(); // Trigger incorrect item event
        customiser.setArrived(true);
    }
}
