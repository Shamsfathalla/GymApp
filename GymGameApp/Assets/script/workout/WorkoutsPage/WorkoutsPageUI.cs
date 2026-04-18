using System;
using System.Collections.Generic; 
using Firebase.Auth; 
using Firebase.Firestore;
using UnityEngine;

public class WorkoutsPageUI : MonoBehaviour
{
    [SerializeField] private Transform workoutCardsContent;
    [SerializeField] private GameObject workoutCardPrefab;
    [SerializeField] private WorkoutDetailsPageUI detailsPageUI;

    private FirebaseUser currentUser; 

    private void OnEnable()
    {
        currentUser = FirebaseAuth.DefaultInstance.CurrentUser; 
        LoadWorkouts(); 
    }

    public async void LoadWorkouts()
    {
        try 
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            QuerySnapshot snapshot = await db.Collection("users").Document(currentUser.UserId).Collection("workouts").GetSnapshotAsync();

            foreach (Transform child in workoutCardsContent.transform) // Clear existing workout cards before populating new ones
            {
                Destroy(child.gameObject); 
            }

            // For each workout document, create a WorkoutData object and create a workout card UI element
            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                if (doc.Exists) 
                {
                    WorkoutData newWorkout = new WorkoutData();  
                    newWorkout.id = doc.Id; 
                    newWorkout.name = doc.GetValue<string>("name"); 
                    newWorkout.exerciseCount = doc.GetValue<int>("exerciseCount");
                    newWorkout.exercises = doc.GetValue<List<Dictionary<string, object>>>("exercises");

                    GameObject card = Instantiate(workoutCardPrefab, workoutCardsContent); 
                    WorkoutCardUI cardUI = card.GetComponent<WorkoutCardUI>();  
                    
                    if (cardUI != null) 
                    {
                        cardUI.Setup(newWorkout, this); 
                    }
                }
            }
        }
        catch (Exception error)
        {
            Debug.LogError("Failed to load workouts: " + error.Message);
        }
    }

    public async void DeleteWorkout(string workoutId)
    {
        if (workoutId == null || workoutId == "") 
        {
            return; 
        }

        try // Delete the workout document from Firestore 
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            await db.Collection("users").Document(currentUser.UserId).Collection("workouts").Document(workoutId).DeleteAsync();
            
            LoadWorkouts(); 
        }
        catch (Exception error)
        {
            Debug.LogError("Failed to delete workout: " + error.Message);
        }
    }

    public void OpenWorkoutDetails(WorkoutData workout)
    {
        if (workout != null) 
        {
            detailsPageUI.Open(workout); 
        }
    }
}