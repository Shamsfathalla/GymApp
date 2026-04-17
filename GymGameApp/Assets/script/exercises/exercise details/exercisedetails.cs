using System;
using System.Collections.Generic; // For using List<T>
using UnityEngine;
using UnityEngine.UI; 
using Firebase.Firestore; 
using TMPro; 

public class exercisedetails : MonoBehaviour
{
    public static exercisedetails Instance; // Singleton instance allowing exercisecard to access this class

    [Header("UI")]
    [SerializeField] private GameObject detailsPage; // The Canvas Panel holding the details UI

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI equipmentText;
    [SerializeField] private TextMeshProUGUI primaryMusclesText;
    [SerializeField] private TextMeshProUGUI secondaryMusclesText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI instructionsText;

    [Header("Images")]
    [SerializeField] private Image exerciseImage; 

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this; // If no instance exists, set it to this one
        }
    }

    public void backButton()
    {
        audioManager.instance.PlayClick();

        detailsPage.SetActive(false); // Hide the details page 
    }

    public async void LoadExercise(string documentId)
    {
        detailsPage.SetActive(true); // Show the details page   

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance; // Get the Firestore database instance
        DocumentReference doc = db.Collection("exercises").Document(documentId); // Reference to the specific document using the ID

        try 
        {
            DocumentSnapshot snapshot = await doc.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                nameText.text = snapshot.GetValue<string>("name"); 
                descriptionText.text = snapshot.GetValue<string>("description"); 
                equipmentText.text = string.Join(", ", snapshot.GetValue<List<string>>("equipment")); 
                primaryMusclesText.text = string.Join(", ", snapshot.GetValue<List<string>>("primaryMuscles")); 
                secondaryMusclesText.text = string.Join(", ", snapshot.GetValue<List<string>>("secondaryMuscles")); 
                
                List<string> instructions = snapshot.GetValue<List<string>>("instructions"); // Get the list of instructions from Firestore

                instructionsText.text = ""; 
                
                for (int i = 0; i < instructions.Count; i++)
                {
                    // Swapped string interpolation ($"") for basic string addition to keep it simple
                    instructionsText.text = instructionsText.text + (i + 1) + ". " + instructions[i] + "\n\n"; 
                }

                string image = snapshot.GetValue<string>("imageUrl"); // Get the image path from Firestore
                exerciseImage.sprite = Resources.Load<Sprite>("ExerciseImages/" + image); 
            }
        }
        catch (Exception error)
        {
            Debug.LogError("Failed to fetch exercise details: " + error.Message);
            nameText.text = "Error loading data.";
        }
    }
}