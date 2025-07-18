using System.Linq;
using TMPro;
using UnityEngine;

public class ParcelCustomiser : MonoBehaviour
{
    [Header("Parcel Elements")]
    [SerializeField] private SpriteRenderer parcelBackground;
    [SerializeField] private SpriteRenderer parcelTape;
    [SerializeField] private SpriteRenderer parcelIdentifier;
    [SerializeField] private TextMeshPro parcelMainLabel;
    [SerializeField] private SpriteRenderer parcelWeightLabel;
    [SerializeField] private SpriteRenderer parcelWeightLabelBackground;
    [SerializeField] private SpriteRenderer parcelSpecialLabel;
    [SerializeField] private SpriteRenderer parcelSpecialLabelBackground;

    [Header("Parcel Sprite Options")]
    [SerializeField] private Sprite damagedParcelSprite;
    [SerializeField] private Sprite normalParcelSprite;
    [SerializeField] private Sprite weightLabelS;
    [SerializeField] private Sprite weightLabelM;
    [SerializeField] private Sprite weightLabelL;
    
    [Header("Parcel Settings")]
    [SerializeField] private bool damagedBox = false;
    [SerializeField] private bool hasWeight = true;
    [SerializeField] private bool hasSpecial = false;
    
    [SerializeField] private Color boxColour;
    [SerializeField] private string boxLabel;

  
    
    [SerializeField] private int parcelDestination = 0; //0 = Default
    [SerializeField] private bool parcelArrived = false;
    [Range(0,2)][SerializeField] private int parcelWeight = -1; // 0 - S, 1 - M, 2 - L

    public void CustomiseParcel(Color colour, string label, int in_weight, bool is_damaged = false)
    {
        boxColour = colour;
        boxLabel = label;
        parcelWeight = in_weight;
        //damagedBox = is_damaged;

        // Handle Damaged
        if (damagedBox)
        {
            parcelBackground.sprite = damagedParcelSprite;
        }
        else
        {
            parcelBackground.sprite = normalParcelSprite;
        }

        // Handle Weight
        if (hasWeight)
        {
            parcelWeightLabelBackground.enabled = true;
            if (parcelWeight == 0)
            {
                parcelWeightLabel.sprite = weightLabelS;
            }
            else if (parcelWeight == 1)
            {
                parcelWeightLabel.sprite = weightLabelM;
            }
            else if (parcelWeight == 2)
            {
                parcelWeightLabel.sprite = weightLabelL;
            }
            else
            {
                parcelWeightLabel.sprite = null;
            }
        }
        else
        {
            parcelWeightLabelBackground.enabled = false;
        }
        
        // Handle Special
        if (hasSpecial)
        {
            parcelSpecialLabelBackground.enabled = true;
        }
        else
        {
            parcelSpecialLabelBackground.enabled = false;
        }
       

        parcelTape.color = boxColour;
        parcelIdentifier.color = boxColour;
        parcelMainLabel.text = boxLabel.First().ToString();
    }

    
    // Getters / Setters
    public void setDestination(int destination)
    {
        parcelDestination = destination;
    }

    public int getDestination()
    {
        return parcelDestination;
    }
    
    public void setArrived(bool arrivedState)
    {
        parcelArrived = arrivedState;
    }

    public void setHasWeight(bool weightState)
    {
        hasWeight = weightState;
    }

    public void setHasSpecial(bool specialState)
    {
        hasSpecial = specialState;
    }

    public bool getArrived()
    {
        return parcelArrived;
    }

    public bool getHasWeight()
    {
        return hasWeight;
    }

    public bool getHasSpecial()
    {
        return hasSpecial;
    }
    
}
