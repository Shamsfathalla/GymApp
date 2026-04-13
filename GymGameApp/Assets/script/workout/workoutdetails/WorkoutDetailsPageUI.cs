using System; 
using System.Collections.Generic; 
using TMPro; 
using Firebase.Auth; 
using Firebase.Firestore; 
using Firebase.Extensions; 
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

    private WorkoutData currentWorkout; // Store the currently displayed workout data
    
    public void backButton() 
    { 
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        detailsPanel.SetActive(false);
    }

    // This method is called by WorkoutBuilderUI after saving changes to refresh the displayed workout details
    public void Open(WorkoutData workout)
    {
        if (workout == null) return; 
        
        currentWorkout = workout; 
        workoutNameText.text = workout.name; 
        detailsPanel.SetActive(true);

        exercisesContent.ClearChildren();
        // Display exercises in the workout
        if (workout.exercises != null)
        {
            // Each exercise is stored as a dictionary with keys like "name", "reps", "sets", etc.
            foreach (Dictionary<string, object> dict in workout.exercises)
            {
                // 1. Spawn the prefab into the scene
                GameObject newCard = Instantiate(workoutExerciseCardPrefab, exercisesContent);

                // 2. Grab the script attached to that specific card
                WorkoutExerciseCardUI cardScript = newCard.GetComponent<WorkoutExerciseCardUI>();

                // 3. Safely pass the data into the script's Setup method
                cardScript.Setup(dict["name"].ToString(), (clickedName) => exerciseButton(clickedName));
            }
        }
    }

    private void exerciseButton(string exerciseName)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser; 
        if (user == null) return;

        // Fetch the current progress and history for the selected exercise
        var exerciseRef = FirebaseFirestore.DefaultInstance.Collection("users").Document(user.UserId).Collection("exerciseProgress").Document(exerciseName); 

        exerciseRef.GetSnapshotAsync().ContinueWithOnMainThread(mainTask => 
        {
            if (mainTask.IsFaulted) return; 
            
            // Prepare the progress data to pass to the detail page
            ExerciseProgressData progressData = new ExerciseProgressData { exerciseName = exerciseName };

            // Get current progress sets
            if (mainTask.Result.Exists)
            {
                List<object> setsObj = mainTask.Result.GetValue<List<object>>("currentSets");
                if (setsObj != null)
                {
                    foreach (Dictionary<string, object> dict in setsObj)
                    {
                        progressData.currentSets.Add(new SetData {
                            reps = Convert.ToInt32(dict["reps"]),
                            weight = Convert.ToSingle(dict["weight"])
                        });
                    }
                }
            }

            // Now fetch the history for this exercise
            exerciseRef.Collection("history").OrderByDescending("savedAt").GetSnapshotAsync().ContinueWithOnMainThread(historyTask => 
            {
                if (historyTask.IsFaulted || historyTask.IsCanceled) return;

                foreach (DocumentSnapshot hDoc in historyTask.Result.Documents)
                {
                    // Convert Firestore data to HistorySession
                    HistorySession session = new HistorySession { date = hDoc.GetValue<string>("date") };
                    // Get sets for this history session
                    List<object> hSetsObj = hDoc.GetValue<List<object>>("sets");
                    if (hSetsObj != null)
                    {
                        foreach (Dictionary<string, object> dict in hSetsObj)
                        {
                            session.sets.Add(new SetData {
                                reps = Convert.ToInt32(dict["reps"]),
                                weight = Convert.ToSingle(dict["weight"])
                            });
                        }
                    }
                    progressData.history.Add(session);
                }

                exerciseDetailPageUI.Open(progressData);
            });
        });
    }

    public void editButton()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        detailsPanel.SetActive(false);
        builderUI.OpenEditButton(currentWorkout, this);
    }
}