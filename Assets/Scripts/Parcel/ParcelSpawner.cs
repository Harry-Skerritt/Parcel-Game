using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParcelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject parcelPrefab;
    [SerializeField] private Transform parcelSpawnPoint;
    [Range(0, 26)][SerializeField] private int maxParcelDestinationLetter;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SpawnParcel();
        }
    }

    private void SpawnParcel()
    {
        if (parcelPrefab != null && parcelSpawnPoint != null)
        {
            GameObject parcel = Instantiate(parcelPrefab, parcelSpawnPoint.position, Quaternion.identity);
            parcel.name = "Parcel_" + System.Guid.NewGuid().ToString().Substring(0, 4);
            Debug.Log("Spawned new parcel: " + parcel.name);
            
            // Create the parcel and get customiser component
            var customiser = parcel.GetComponent<ParcelCustomiser>();
        
            // Set the parcels destiantion
            int destination = Random.Range(1, maxParcelDestinationLetter+1);
            customiser.setDestination(destination);
            string parcelLabel = NumberToLetter(destination).ToString();
            
            // Set the weight
            int weight = Random.Range(0, 3);
        
            // Customise Colour -> Change to being a set colour per desitnation.
            Color parcelColour = Random.ColorHSV();
            customiser.CustomiseParcel(parcelColour, parcelLabel, weight);
            
        }
        else
        {
            Debug.LogError("Parcel Spawner is null!");
        }
        
        
    }
    
    
    static char NumberToLetter(int number)
    {
        return (char)('A' + number - 1);
    }
}
