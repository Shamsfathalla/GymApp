using System;
using System.Collections.Generic; 
using TMPro; 
using UnityEngine;

public class WorkoutBuilderUI : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private WorkoutDatabaseManager databaseManager;

    [Header("UI Panels")]
    [SerializeField] private GameObject builderPanel;
    [SerializeField] private GameObject addExercisePanel;
    
    [Header("Inputs & Lists")]
    [SerializeField] private TMP_InputField workoutNameInput;
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private Transform addedExercisesContent;
    [SerializeField] private Transform popupExercisesContent;
    
    [Header("Prefabs & References")]
    [SerializeField] private GameObject addExercisePrefab;
    [SerializeField] private GameObject deleteExercisePrefab;
    [SerializeField] private WorkoutsPageUI workoutsPageUI;

    private List<ExerciseData> allExercises = new List<ExerciseData>(); 
    private List<string> selectedExercises = new List<string>(); 
    
    private string editingWorkoutId; // null if creating new workout, otherwise contains the ID of the workout being edited
    private WorkoutDetailsPageUI linkedDetailsPage; 

    private void Start() 
    { 
        searchInput.onValueChanged.AddListener(_ => RefreshPopupList()); 
    } 

    // BUTTONS

    public void OpenCreateButton() 
    { 
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        SetupBuilder(null, null, ""); 
    }

    public void OpenEditButton(WorkoutData workout, WorkoutDetailsPageUI detailsPage) 
    {
        SetupBuilder(workout.id, detailsPage, workout.name);  // Pre-fill the name input, and store the workout ID and details page reference for later
        
        if (workout.exercises != null)
        {
            foreach (Dictionary<string, object> ex in workout.exercises) // The workout data only has exercise names, so we just extract those and add them to the selected list
            {
                string exerciseName = ex["name"].ToString(); // Extract the exercise name from the dictionary
                selectedExercises.Add(exerciseName); // Add the exercise name to the selected exercises list
            }
        }
        RefreshAddedExercisesList(); // Refresh the added exercises list to show the pre-filled exercises
    }

    public void AddPanelButton(bool show) 
    { 
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        addExercisePanel.SetActive(show); 
    }
    
    public void DiscardWorkoutButton() 
    { 
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        builderPanel.SetActive(false); 
    }

    // Setup the builder for either creating a new workout (id = null) or editing an existing one (id = workout's ID). Also takes in the details page reference so we can update it after saving, and the workout name to pre-fill the name input if editing.
    private void SetupBuilder(string id, WorkoutDetailsPageUI page, string name)
    {
        editingWorkoutId = id;
        linkedDetailsPage = page;
        workoutNameInput.text = name;
        searchInput.text = ""; 
        
        selectedExercises.Clear(); 
        RefreshAddedExercisesList(); 
        
        builderPanel.SetActive(true);
        addExercisePanel.SetActive(false);
        
        // Ask the database manager to load, and tell it to run "OnExercisesLoaded" when done
        databaseManager.LoadExercises(OnExercisesLoaded);
    }

    // Load the full list of exercises from the database, store it locally, and refresh the popup list to show them
    private void OnExercisesLoaded(List<ExerciseData> loadedList)
    {
        allExercises = loadedList;
        RefreshPopupList();
    }

    // ADDING AND REMOVING EXERCISES
    public void AddExercise(ExerciseData exercise) 
    { 
        // Only add the exercise if it's not already in the selected list
        if (!selectedExercises.Contains(exercise.name))
        {
            selectedExercises.Add(exercise.name); 
            RefreshAddedExercisesList();
            RefreshPopupList();
        }
    }

    public void RemoveExercise(string exerciseName) 
    { 
        // Only remove the exercise if it's actually in the selected list 
        if (selectedExercises.Remove(exerciseName))
        {
            RefreshAddedExercisesList();
            RefreshPopupList();
        }
    }

    // FIREBASE SAVING COMMUNICATION 
    public void SaveWorkoutButton()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        string workoutName = workoutNameInput.text.Trim(); // Get the workout name from the input and trim any extra whitespace
        if (string.IsNullOrEmpty(workoutName)) return;  // Don't allow saving if the workout name is empty

        // Convert the list of selected exercise names into the format expected by the database script (a list of dictionaries with "name" keys)
        List<Dictionary<string, object>> exerciseList = new List<Dictionary<string, object>>();
        // The workout data only has exercise names, so we just convert our list of selected exercise names into the expected format
        foreach (string exName in selectedExercises)
        {
            exerciseList.Add(new Dictionary<string, object> { { "name", exName } });
        }
        
        // Pass the data to the database script, and tell it to run FinishSaving when done!
        databaseManager.SaveWorkout(editingWorkoutId, workoutName, exerciseList, FinishSaving);
    }

    private void FinishSaving()
    {
        if (workoutsPageUI != null) workoutsPageUI.LoadWorkouts();  // Tell the workouts page to refresh its workout list, so the new/edited workout will show up there immediately
        builderPanel.SetActive(false); 

        if (linkedDetailsPage != null) 
        {
            // Prepare the list of exercises for the details page
            List<Dictionary<string, object>> savedExercises = new List<Dictionary<string, object>>();
            foreach (string name in selectedExercises)
            {
                // The details page also expects a list of dictionaries with "name" keys, so we convert our list of selected exercise names into that format
                savedExercises.Add(new Dictionary<string, object> { { "name", name } });
            }

            // Open the details page with the saved workout data (using the existing workout ID if editing, or null if creating new)
            linkedDetailsPage.Open(new WorkoutData { 
                id = editingWorkoutId, 
                name = workoutNameInput.text.Trim(), 
                exercises = savedExercises 
            }); 
        }
    }

    // REFRESHING UI LISTS
    private void RefreshAddedExercisesList()
    {
        addedExercisesContent.ClearChildren(); 
        // Create a new exercise card for each selected exercise
        foreach (string name in selectedExercises)
        {
            GameObject cardObj = Instantiate(deleteExercisePrefab, addedExercisesContent); // Create the delete exercise prefab
            cardObj.GetComponent<ExerciseCardUI2>().Setup(new ExerciseData { name = name }, this, true); // Set up the exercise card with the exercise data
        }
    }

    private void RefreshPopupList()
    {
        popupExercisesContent.ClearChildren();
        string search = searchInput.text.Trim().ToLower(); 
        
        foreach (ExerciseData ex in allExercises) 
        {   
            // Check if the exercise matches the search criteria
            if (string.IsNullOrEmpty(search) || (ex.name != null && ex.name.ToLower().Contains(search)))
            {
                bool isSelected = selectedExercises.Contains(ex.name); // Check if the exercise is already in the selected list
                GameObject prefabToUse = isSelected ? deleteExercisePrefab : addExercisePrefab; // Use the delete prefab if the exercise is already selected, otherwise use the add prefab

                GameObject cardObj = Instantiate(prefabToUse, popupExercisesContent); // Create the chosen prefab
                cardObj.GetComponent<ExerciseCardUI2>().Setup(ex, this, isSelected); // Set up the exercise card with the exercise data
            }
        }
    }
}