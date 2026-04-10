using System.Collections.Generic; 
using Firebase.Auth; 
using Firebase.Firestore;
using Firebase.Extensions; 
using UnityEngine;

public class WorkoutsPageUI : MonoBehaviour
{
    [SerializeField] private Transform workoutCardsContent;
    [SerializeField] private GameObject workoutCardPrefab;
    [SerializeField] private WorkoutDetailsPageUI detailsPageUI;

    private FirebaseUser currentUser; // Store the current user to avoid repeated calls to FirebaseAuth.DefaultInstance.CurrentUser

    private void OnEnable()
    {
        currentUser = FirebaseAuth.DefaultInstance.CurrentUser; // Cache the current user when the page is enabled
        LoadWorkouts(); 
    }

    public void LoadWorkouts()
    {
        if (currentUser == null) return; 

        // Fetch workouts from Firestore
        FirebaseFirestore.DefaultInstance.Collection("users").Document(currentUser.UserId).Collection("workouts").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted) return; 

            workoutCardsContent.ClearChildren();  // Clear existing workout cards before loading new ones

            // Standard loop through documents
            foreach (DocumentSnapshot doc in task.Result.Documents)
            {
                if (doc.Exists) 
                {
                    WorkoutData newWorkout = new WorkoutData();  // Create a new WorkoutData instance for each document
                    newWorkout.id = doc.Id; 

                    // Directly grab the values
                    newWorkout.name = doc.GetValue<string>("name"); 
                    newWorkout.exerciseCount = doc.GetValue<int>("exerciseCount");
                    
                    // Get exercises as a list of dictionaries and assign to the workout dataet 
                    newWorkout.exercises = doc.GetValue<List<Dictionary<string, object>>>("exercises");

                    GameObject card = Instantiate(workoutCardPrefab, workoutCardsContent); // Create a new card for each workout
                    WorkoutCardUI cardUI = card.GetComponent<WorkoutCardUI>();  
                    
                    if (cardUI != null) {
                        cardUI.Setup(newWorkout, this); // Pass the workout data and reference to this page for callbacks
                    }
                }
            }
        });
    }

    public void DeleteWorkout(string workoutId)
    {
        if (string.IsNullOrWhiteSpace(workoutId)) return; // Validate workoutId before attempting deletion

        FirebaseFirestore.DefaultInstance.Collection("users").Document(currentUser.UserId).Collection("workouts")
          .Document(workoutId).DeleteAsync().ContinueWithOnMainThread(task => // After deletion, reload workouts to reflect changes
          {
              if (!task.IsFaulted) LoadWorkouts(); 
          });
    }

    public void OpenWorkoutDetails(WorkoutData workout)
    {
        if (workout != null) detailsPageUI.Open(workout); // Open the details page with the selected workout data
    }
}