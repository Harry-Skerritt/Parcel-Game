using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BinController : DropLocation
{
    [Header("On Hover")]
    private Vector3 startScale;
    [SerializeField] private Vector3 hoverScale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float transitionDuration = 0.5f;
    private Coroutine scaleCoroutine;
    
    private Color defaultColor;
   
    
    private void Awake()
    {
        startScale = transform.localScale;
        
        locationSprite = GetComponent<SpriteRenderer>();
        if (locationSprite != null)
        {
            defaultColor = locationSprite.color;
        }
       
        if(onCorrectItemDropped == null) onCorrectItemDropped = new UnityEvent();
        if(onIncorrectItemDropped == null) onIncorrectItemDropped = new UnityEvent();

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
                if (scaleCoroutine != null)
                {
                    StopCoroutine(scaleCoroutine);
                }
                scaleCoroutine = StartCoroutine(ScaleOverTime(startScale, hoverScale));   
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

                
                if (scaleCoroutine != null)
                {
                    StopCoroutine(scaleCoroutine);
                }
                scaleCoroutine = StartCoroutine(ScaleOverTime(hoverScale, startScale));

            }
        }
    }
    
    private IEnumerator ScaleOverTime(Vector3 fromScale, Vector3 toScale)
    {
        float timer = 0f;
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = timer / transitionDuration;
            t = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = Vector3.Lerp(fromScale, toScale, t);
            yield return null;
        }
        transform.localScale = toScale;
        scaleCoroutine = null;
    }
    
    public bool attemptDrop(DraggableObject droppedObject)
    {
        bool isCorrectDrop = false;
        ParcelCustomiser customiser = droppedObject.gameObject.GetComponent<ParcelCustomiser>();

        if (customiser != null)
        {
            // Handle Bin stuff
            if (customiser.getDamaged() && (customiser.getDestination() == destinationIdentifier))
            {                
                correctItem(droppedObject, customiser);
                isCorrectDrop = true;
            }
            else
            {
                incorrectItem(droppedObject, customiser);
                isCorrectDrop = true; // Need to refactor this as it isnt
            } 
        }
        else
        {
            Debug.LogWarning($"AttemptDrop: DraggableObject {droppedObject.gameObject.name} does not have a ParcelCustomiser component.");
            isCorrectDrop = false;
        }
        
        if (locationSprite != null)
        {
            setLocationColour(defaultColor);
        }

        return isCorrectDrop;
    }
}
