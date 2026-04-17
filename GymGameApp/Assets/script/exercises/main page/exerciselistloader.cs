using UnityEngine;
using Firebase.Firestore;
using TMPro; 

public class exerciselistloader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform scrollViewContent; // ScrollView for exercise cards
    [SerializeField] private GameObject cardPrefab; // Prefab for the exercise card

    [Header("Muscle Category")]
    [SerializeField] private string Category = "chest"; // The muscle category to display in page

    void Start()
    {
        LoadExercises(Category); // Load exercises for the specified category when the scene starts
    }

    public async void LoadExercises(string category)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance; // Get Firestore instance

        Query query = db.Collection("exercises").WhereEqualTo("category", category); // Query exercises by muscle category

        // Using await to pause until data is fetched, avoiding complex callbacks
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        foreach (Transform child in scrollViewContent.transform) 
        {
            Destroy(child.gameObject); // Clear out existing cards
        }

        foreach (DocumentSnapshot document in snapshot.Documents) // Loop through each exercise document
        {
            string exerciseName = document.GetValue<string>("name"); // Get exercise name from Firestore
            string documentId = document.Id; // Get the document ID for later use

            // Create a new card for each exercise and set it up
            GameObject card = Instantiate(cardPrefab, scrollViewContent); 
            exerciseCard cardScript = card.GetComponent<exerciseCard>(); 
            cardScript.Setup(exerciseName, documentId); 
        }
    }
}