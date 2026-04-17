using System;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class WorkoutDatabaseManager : MonoBehaviour
{
    // Loads all active exercises from the database and returns them in a list 
    public async void LoadExercises(Action<List<ExerciseData>> onLoaded) 
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        
        // We only want to load active exercises, so we use a query to filter them
        Query query = db.Collection("exercises").WhereEqualTo("isActive", true);
        
        // await pauses this method until the database finishes fetching, reading top-to-bottom cleanly
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        
        List<ExerciseData> loadedList = new List<ExerciseData>(); 
        
        // We loop through the returned documents and create ExerciseData objects from them
        foreach (DocumentSnapshot doc in snapshot.Documents) 
        {
            ExerciseData newExercise = new ExerciseData();
            newExercise.name = doc.GetValue<string>("name");
            loadedList.Add(newExercise);
        }
        
        if (onLoaded != null) 
        {
            onLoaded(loadedList);
        }
    }

    // Saves a workout to the database. If workoutId is empty or null, it creates a new workout. Otherwise, it updates the existing workout with the given ID.
    public async void SaveWorkout(string workoutId, string workoutName, List<Dictionary<string, object>> exerciseList, Action onFinished)
    {
        FirebaseUser currentUser = FirebaseAuth.DefaultInstance.CurrentUser; 

        Dictionary<string, object> newWorkoutData = new Dictionary<string, object>();
        newWorkoutData.Add("name", workoutName);
        newWorkoutData.Add("exerciseCount", exerciseList.Count);
        newWorkoutData.Add("exercises", exerciseList);
        newWorkoutData.Add("updatedAt", Timestamp.GetCurrentTimestamp());

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference workoutsRef = db.Collection("users").Document(currentUser.UserId).Collection("workouts"); 

        if (workoutId == "" || workoutId == null)
        {
            // It is a NEW workout
            newWorkoutData.Add("createdAt", Timestamp.GetCurrentTimestamp()); 
            await workoutsRef.AddAsync(newWorkoutData);
        }
        else
        {
            // It is an EXISTING workout
            await workoutsRef.Document(workoutId).UpdateAsync(newWorkoutData);
        }

        if (onFinished != null) 
        {
            onFinished();
        }
    }
}