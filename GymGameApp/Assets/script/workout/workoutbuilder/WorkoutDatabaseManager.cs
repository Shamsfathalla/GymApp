using System;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class WorkoutDatabaseManager : MonoBehaviour
{
    // The "Action<List<ExerciseData>>" tells this method to hand back a list when it's done.
    public void LoadExercises(Action<List<ExerciseData>> onLoaded)
    {
        var db = FirebaseFirestore.DefaultInstance;
        db.Collection("exercises").WhereEqualTo("isActive", true).GetSnapshotAsync().ContinueWithOnMainThread(task => 
        { 
            if (task.IsFaulted) return;
            
            List<ExerciseData> loadedList = new List<ExerciseData>();
            foreach (DocumentSnapshot doc in task.Result.Documents)
            {
                loadedList.Add(new ExerciseData { name = doc.GetValue<string>("name") });
            }
            
            // This is the walkie-talkie! It sends the filled list back to the UI script.
            if (onLoaded != null) onLoaded(loadedList);
        });
    }

    // The "Action" here just means "Let me know when the save is completely finished."
    public void SaveWorkout(string workoutId, string workoutName, List<Dictionary<string, object>> exerciseList, Action onFinished)
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser; 
        if (currentUser == null) return; 

        var newWorkoutData = new Dictionary<string, object>
        {
            { "name", workoutName }, 
            { "exerciseCount", exerciseList.Count }, 
            { "exercises", exerciseList }, 
            { "updatedAt", Timestamp.GetCurrentTimestamp() } 
        };

        var db = FirebaseFirestore.DefaultInstance;
        var workoutsRef = db.Collection("users").Document(currentUser.UserId).Collection("workouts"); 

        if (string.IsNullOrEmpty(workoutId))
        {
            // It is a NEW workout
            newWorkoutData.Add("createdAt", Timestamp.GetCurrentTimestamp()); 
            workoutsRef.AddAsync(newWorkoutData).ContinueWithOnMainThread(task => 
            {
                if (onFinished != null) onFinished(); 
            });
        }
        else
        {
            // It is an EXISTING workout
            workoutsRef.Document(workoutId).UpdateAsync(newWorkoutData).ContinueWithOnMainThread(task => 
            {
                if (onFinished != null) onFinished();
            });
        }
    }
}