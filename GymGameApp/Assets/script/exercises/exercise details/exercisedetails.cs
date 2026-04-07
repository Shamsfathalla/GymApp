using System.Collections.Generic; // For using List<T>
using UnityEngine;
using UnityEngine.UI; // For using UI components like Button
using Firebase.Firestore; // For using Firestore database features
using Firebase.Extensions; // Ensures Firebase tasks run safely on Unity's main thread
using TMPro; // For TextMeshPro UI elements

public class exercisedetails : MonoBehaviour
{
    public static exercisedetails Instance; // Singleton instance allowing exercisecard to access this class

    [Header("UI")]
    public GameObject detailsPage; // The Canvas Panel holding the details UI

    [Header("Text")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI equipmentText;
    public TextMeshProUGUI primaryMusclesText;
    public TextMeshProUGUI secondaryMusclesText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI instructionsText;

    [Header("Images")]
    public Image exerciseImage; 

    void Awake()
    {
        if (Instance == null) Instance = this; // If no instance exists, set it to this one
    }

    public void backButton()
        {
            detailsPage.SetActive(false); // Hide the details page 
        }

    public void LoadExercise(string documentId)
    {
        detailsPage.SetActive(true); // Show the details page   

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance; // Get the Firestore database instance
        DocumentReference doc = db.Collection("exercises").Document(documentId); // Reference to the specific document using the ID

        // Requests a snapshot (image copy of data) of what the database looks like right now. 
        // Runs in the background (async) so it doesn't block the rest of the code.
        doc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to fetch exercise details.");
                nameText.text = "Error loading data.";
                return;
            }

            DocumentSnapshot snapshot = task.Result; // Get the document snapshot

            if (snapshot.Exists)
            {
                nameText.text = snapshot.GetValue<string>("name"); 
                descriptionText.text = snapshot.GetValue<string>("description"); 
                equipmentText.text = string.Join(", ", snapshot.GetValue<List<string>>("equipment")); // Combine equipment list into a clean string
                primaryMusclesText.text = string.Join(", ", snapshot.GetValue<List<string>>("primaryMuscles")); // Combine primary muscles list into a clean string
                secondaryMusclesText.text = string.Join(", ", snapshot.GetValue<List<string>>("secondaryMuscles")); // Combine secondary muscles list into a clean string
                List<string> instructions = snapshot.GetValue<List<string>>("instructions"); // Get the list of instructions from Firestore

                instructionsText.text = ""; // Clear the instructions text
                for (int i = 0; i < instructions.Count; i++)
                {
                    instructionsText.text += ($"{i + 1}. {instructions[i]}\n\n"); // Format each instruction with a number
                }

                string image = snapshot.GetValue<string>("imageUrl"); // Get the image path from Firestore
                exerciseImage.sprite = Resources.Load<Sprite>("ExerciseImages/" + image); // Load the image from the Resources folder using the path
            }
        });
    }
}