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
        searchInput.onValueChanged.AddListener(OnSearchValueChanged); // Load exercises when the builder is opened for the first time
    } 

    private void OnSearchValueChanged(string value)
    {
        RefreshPopupList();
    }

    // BUTTONS
    public void OpenCreateButton() 
    { 
        audioManager.instance.PlayClick();

        SetupBuilder(null, null, ""); 
    }

    public void OpenEditButton(WorkoutData workout, WorkoutDetailsPageUI detailsPage) 
    {
        SetupBuilder(workout.id, detailsPage, workout.name);  // Load selected exercises from the workout data
        
        if (workout.exercises != null)
        {
            foreach (Dictionary<string, object> ex in workout.exercises) // Convert exercise dictionaries to list of exercise names
            {
                string exerciseName = ex["name"].ToString(); 
                selectedExercises.Add(exerciseName); 
            }
        }
        RefreshAddedExercisesList(); 
    }

    public void AddPanelButton(bool show) 
    { 
        audioManager.instance.PlayClick();

        addExercisePanel.SetActive(show); 
    }
    
    public void DiscardWorkoutButton() 
    { 
        audioManager.instance.PlayClick();

        builderPanel.SetActive(false); 
    }

    private void SetupBuilder(string id, WorkoutDetailsPageUI page, string name)
    {
        editingWorkoutId = id;
        linkedDetailsPage = page;
        workoutNameInput.text = name;
        searchInput.text = ""; 
        
        selectedExercises.Clear(); // Clear selected exercises list before loading new workout data
        RefreshAddedExercisesList(); 
        
        builderPanel.SetActive(true);
        addExercisePanel.SetActive(false);
        
        databaseManager.LoadExercises(OnExercisesLoaded);
    }

    private void OnExercisesLoaded(List<ExerciseData> loadedList)
    {
        allExercises = loadedList;
        RefreshPopupList();
    }

    public void AddExercise(ExerciseData exercise) 
    { 
        if (selectedExercises.Contains(exercise.name) == false)
        {
            selectedExercises.Add(exercise.name); 
            RefreshAddedExercisesList();
            RefreshPopupList();
        }
    }

    public void RemoveExercise(string exerciseName) 
    { 
        if (selectedExercises.Remove(exerciseName) == true)
        {
            RefreshAddedExercisesList();
            RefreshPopupList();
        }
    }

    public void SaveWorkoutButton()
    {
        audioManager.instance.PlayClick();

        string workoutName = workoutNameInput.text;  
        
        if (workoutName == null || workoutName == "") 
        {
            return;  
        }

        List<Dictionary<string, object>> exerciseList = new List<Dictionary<string, object>>(); // Convert list of exercise names to list of exercise dictionaries for saving
        
        // For loop to convert exercise names to dictionaries with "name" key
        foreach (string exName in selectedExercises)
        {
            Dictionary<string, object> newExercise = new Dictionary<string, object>();
            newExercise.Add("name", exName);

            exerciseList.Add(newExercise);
        }
        
        databaseManager.SaveWorkout(editingWorkoutId, workoutName, exerciseList, FinishSaving); // Save workout to database, then call FinishSaving to update UI and return to details page
    }

    private void FinishSaving()
    {
        if (workoutsPageUI != null) 
        {
            workoutsPageUI.LoadWorkouts();  
        }

        builderPanel.SetActive(false); 

        if (linkedDetailsPage != null) 
        {
            List<Dictionary<string, object>> savedExercises = new List<Dictionary<string, object>>();
            
            // For loop to convert list of exercise names to dictionaries with "name" key for passing to details page
            foreach (string name in selectedExercises)
            {
                Dictionary<string, object> newExercise = new Dictionary<string, object>();
                newExercise.Add("name", name);
                savedExercises.Add(newExercise);
            }

            WorkoutData savedData = new WorkoutData();
            savedData.id = editingWorkoutId;
            savedData.name = workoutNameInput.text;
            savedData.exercises = savedExercises;

            linkedDetailsPage.Open(savedData); 
        }
    }

    private void RefreshAddedExercisesList()
    {
        foreach (Transform child in addedExercisesContent.transform) 
        {
            Destroy(child.gameObject); 
        }

        // Create exercise cards for each selected exercise in the added exercises list using the delete prefab
        foreach (string name in selectedExercises)
        {
            GameObject cardObj = Instantiate(deleteExercisePrefab, addedExercisesContent); 
            
            ExerciseData emptyData = new ExerciseData();
            emptyData.name = name;

            cardObj.GetComponent<ExerciseCardUI2>().Setup(emptyData, this, true); 
        }
    }

    private void RefreshPopupList()
    {
        foreach (Transform child in popupExercisesContent.transform) 
        {
            Destroy(child.gameObject); 
        }

        string search = searchInput.text.ToLower(); 
        
        foreach (ExerciseData ex in allExercises) 
        {   
            if (search == "" || search == null || (ex.name != null && ex.name.ToLower().Contains(search)))
            {
                bool isSelected = selectedExercises.Contains(ex.name); 
                GameObject prefabToUse;
                
                // If the exercise is already selected, show the delete prefab in the popup list, otherwise show the add prefab
                if (isSelected == true)
                {
                    prefabToUse = deleteExercisePrefab;
                }
                else
                {
                    prefabToUse = addExercisePrefab;
                }

                GameObject cardObj = Instantiate(prefabToUse, popupExercisesContent); 
                cardObj.GetComponent<ExerciseCardUI2>().Setup(ex, this, isSelected); 
            }
        }
    }
}