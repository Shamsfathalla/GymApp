using System; 
using System.Collections.Generic; 
using TMPro; 
using Firebase.Auth; 
using Firebase.Firestore; 
using UnityEngine;

public class WorkoutDetailsPageUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private TMP_Text workoutNameText;
    [SerializeField] private Transform exercisesContent;
    [SerializeField] private GameObject workoutExerciseCardPrefab;
    [SerializeField] private WorkoutBuilderUI builderUI;
    [SerializeField] private ExerciseDetailPageUI exerciseDetailPageUI; 

    private WorkoutData currentWorkout; 
    
    public void backButton() 
    { 
        audioManager.instance.PlayClick();

        detailsPanel.SetActive(false);
    }

    public void Open(WorkoutData workout)
    {
        if (workout == null) 
        {
            return; 
        }
        
        // Set the current workout data and update the UI
        currentWorkout = workout; 
        workoutNameText.text = workout.name; 
        detailsPanel.SetActive(true);

        // Clear existing exercise cards before populating new ones
        foreach (Transform child in exercisesContent.transform) 
        {
            Destroy(child.gameObject); 
        }
        
        if (workout.exercises != null)
        {
            foreach (Dictionary<string, object> dict in workout.exercises)
            {
                GameObject newCard = Instantiate(workoutExerciseCardPrefab, exercisesContent);
                WorkoutExerciseCardUI cardScript = newCard.GetComponent<WorkoutExerciseCardUI>();

                // Replaced the lambda callback with just directly passing the method name
                cardScript.Setup(dict["name"].ToString(), exerciseButton);
            }
        }
    }

    private async void exerciseButton(string exerciseName)
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser; 
        
        if (user == null) 
        {
            return;
        }

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference exerciseRef = db.Collection("users").Document(user.UserId).Collection("exerciseProgress").Document(exerciseName); 

        try 
        {
            // First await for current progress sets
            DocumentSnapshot mainDoc = await exerciseRef.GetSnapshotAsync();

            // Create the ExerciseProgressData object to pass to the details page
            ExerciseProgressData progressData = new ExerciseProgressData();
            progressData.exerciseName = exerciseName;

            if (mainDoc.Exists) // Check if the document exists before trying to read data
            {
                List<object> setsObj = mainDoc.GetValue<List<object>>("currentSets"); // Check if the "currentSets" field exists and is not null
                
                if (setsObj != null)
                {
                    foreach (Dictionary<string, object> dict in setsObj)
                    {
                        SetData newSet = new SetData();
                        newSet.reps = Convert.ToInt32(dict["reps"]);
                        newSet.weight = Convert.ToSingle(dict["weight"]);

                        progressData.currentSets.Add(newSet);
                    }
                }
            }

            // Second await for history sets
            QuerySnapshot historySnapshot = await exerciseRef.Collection("history").OrderByDescending("savedAt").GetSnapshotAsync();

            // For loop to convert history sessions into HistorySession objects and add them to the progressData.history list
            foreach (DocumentSnapshot hDoc in historySnapshot.Documents)
            {
                HistorySession session = new HistorySession();
                session.date = hDoc.GetValue<string>("date");
                
                List<object> hSetsObj = hDoc.GetValue<List<object>>("sets");
                
                if (hSetsObj != null)
                {
                    foreach (Dictionary<string, object> dict in hSetsObj) // Loop through each set in the history session and convert it to a SetData object, then add it to the session.sets list
                    {
                        SetData historySet = new SetData();
                        historySet.reps = Convert.ToInt32(dict["reps"]);
                        historySet.weight = Convert.ToSingle(dict["weight"]);

                        session.sets.Add(historySet);
                    }
                }
                
                progressData.history.Add(session);
            }

            exerciseDetailPageUI.Open(progressData);
        }
        catch (Exception error)
        {
            Debug.LogError("Error loading exercise details: " + error.Message);
        }
    }

    public void editButton()
    {
        audioManager.instance.PlayClick();

        detailsPanel.SetActive(false);
        builderUI.OpenEditButton(currentWorkout, this);
    }
}