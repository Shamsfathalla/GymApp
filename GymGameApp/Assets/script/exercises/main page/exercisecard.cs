using UnityEngine;
using TMPro; 
using UnityEngine.UI; 

public class exerciseCard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI exerciseNameText;
    [SerializeField] private Button viewButton;

    private string documentId; // Store the Firestore document ID for this exercise

    public void Setup(string exerciseName, string id)
    {
        exerciseNameText.text = exerciseName;
        documentId = id;

        viewButton.onClick.RemoveAllListeners(); // Clear any existing listeners to prevent multiple calls
        viewButton.onClick.AddListener(Details); // Add listener to the view button to load exercise details
    }

    private void Details()
    {
        if (audioManager.instance != null)
        {
            audioManager.instance.PlayClick(); // Play click sound when the view button is pressed
        }

        Debug.Log("Load details for document ID: " + documentId);
        
        exercisedetails.Instance.LoadExercise(documentId); // Method to load exercise details using the document ID
    }
}