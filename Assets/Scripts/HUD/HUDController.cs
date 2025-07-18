using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("HUD Fields")]
    [SerializeField] private TextMeshProUGUI routedText;
    [SerializeField] private TextMeshProUGUI misroutedText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI playerLevelText;

    [Header("XP Amounts")] 
    [SerializeField] private int amtPerCorrect = 10;
    [SerializeField] private int amtPerIncorrect = 2;
    [SerializeField] private int amtPerLevel = 50; // Make this dynamic
   
    private int routedAmt = 0;
    private int misroutedAmt = 0;
    private int accuracy = 0;
    private int xp = 0;
    private int playerLevel = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        updateScreen();
    }

    public void increaseRouted(int amount)
    {
        routedAmt += amount;
        xp += amtPerCorrect;
        calculateAndUpdate();
    }

    public void increaseMisrouted(int amount)
    {
        misroutedAmt += amount;
        xp += amtPerIncorrect;
        calculateAndUpdate();
    }
    
    private void calculateAccuracy()
    {
        int totalParcels = routedAmt + misroutedAmt;
        accuracy = (totalParcels / routedAmt) * 100;
    }

    private void calculateLevel()
    {
        playerLevel = (xp / amtPerLevel) + 1;
    }


    private void calculateAndUpdate()
    {
        calculateAccuracy();
        calculateLevel();
        updateScreen();
    }
    private void updateScreen()
    {
        if (routedText != null) routedText.text = routedAmt.ToString();
        if (misroutedText != null) misroutedText.text = misroutedAmt.ToString();
        if (accuracyText != null) accuracyText.text = accuracy.ToString() + "%";
        if (xpText != null) xpText.text = xp.ToString();
        if (playerLevelText != null) playerLevelText.text = playerLevel.ToString();

    }
}
