using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ParcelCustomiser : MonoBehaviour
{
    [Header("Parcel Elements")]
    [SerializeField] private SpriteRenderer parcelBackground;
    [SerializeField] private SpriteRenderer parcelTape;
    [SerializeField] private SpriteRenderer parcelIdentifier;
    [SerializeField] private TextMeshPro parcelLabel;

    [Header("Parcel Sprite Options")]
    [SerializeField] private Sprite damagedParcelSprite;
    [SerializeField] private Sprite normalParcelSprite;
    
    [Header("Parcel Settings")]
    [SerializeField] private bool damagedBox = false;
    [SerializeField] private Color boxColour;
    [SerializeField] private string boxLabel;
    
    [SerializeField] private int parcelDestination = 0; //0 = Default
    [SerializeField] private bool parcelArrived = false;
    
    
    public void CustomiseParcel(Color colour, string label, bool is_damaged = false)
    {
        boxColour = colour;
        boxLabel = label;
        //damagedBox = is_damaged;
        
        if (damagedBox)
        {
            parcelBackground.sprite = damagedParcelSprite; 
        }
        else
        {
            parcelBackground.sprite = normalParcelSprite; 
        }

        parcelTape.color = boxColour;
        parcelIdentifier.color = boxColour;
        parcelLabel.text = boxLabel.First().ToString();
    }

    public void setDestination(int destination)
    {
        parcelDestination = destination;
    }

    public void setArrived()
    {
        parcelArrived = true;
    }
    
}
