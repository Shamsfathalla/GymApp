using UnityEngine;
using Firebase.Firestore; // For using Firestore database
using Firebase.Extensions; // Ensures Firebase tasks run safely on Unity's main thread
using TMPro; // For TextMeshPro UI elements

public class exerciselistloader : MonoBehaviour
{
    [Header("UI")]
    public Transform scrollViewContent; // ScrollView for exercise cards
    public GameObject cardPrefab; // Prefab for the exercise card

    [Header("Muscle Category")]
    public string Category = "chest"; // The muscle category to display in page

    void Start()
    {
        LoadExercises(Category); // Load exercises for the specified category when the scene starts
    }

    public void LoadExercises(string category)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance; // Get Firestore instance

        Query query = db.Collection("exercises").WhereEqualTo("category", category); // Query exercises by muscle category

        // Requests a snapshot (image copy of data) of what the database looks like right now. 
        // Runs in the background (async) so it doesn't block the rest of the code.
        query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error fetching exercises: " + task.Exception);
                return;
            }

            QuerySnapshot snapshot = task.Result; // Get the query results

            foreach (DocumentSnapshot document in snapshot.Documents) // Loop through each exercise document
            {
                string exerciseName = document.GetValue<string>("name"); // Get exercise name from Firestore
                string documentId = document.Id; // Get the document ID for later use

                // Create a new card for each exercise and set it up
                GameObject card = Instantiate(cardPrefab, scrollViewContent); // Create the card prefab as a child of the scroll view content
                exerciseCard cardScript = card.GetComponent<exerciseCard>(); // Get the exerciseCard script component from the created card
                cardScript.Setup(exerciseName, documentId); // Pass the exercise name and document ID to the card
            }
        });
    }
}