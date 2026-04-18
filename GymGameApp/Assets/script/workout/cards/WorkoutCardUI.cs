using TMPro; 
using UnityEngine; 
using UnityEngine.UI;

public class WorkoutCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text workoutNameText;
    [SerializeField] private Button accessButton;
    [SerializeField] private Button deleteButton;

    private WorkoutData cachedData; // Cache the workout data for later use
    private WorkoutsPageUI cachedPageUI; // Cache the page UI reference for later use

    public void Setup(WorkoutData data, WorkoutsPageUI pageUI)
    {
        cachedData = data;
        cachedPageUI = pageUI;

        // Set workout name with a fallback to "Unnamed Workout" if the name is null or empty
        if (data != null && data.name != null && data.name != "")
        {
            workoutNameText.text = data.name;
        }
        else
        {
            workoutNameText.text = "Unnamed Workout";
        }

        accessButton.onClick.RemoveAllListeners(); 
        accessButton.onClick.AddListener(OnAccessClicked);

        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(OnDeleteClicked);
    }

    private void OnAccessClicked()
    {
        if (audioManager.instance != null)
        {
            audioManager.instance.PlayClick();
        }

        if (cachedPageUI != null && cachedData != null)
        {
            cachedPageUI.OpenWorkoutDetails(cachedData);
        }
    }

    private void OnDeleteClicked()
    {
        if (audioManager.instance != null)
        {
            audioManager.instance.PlayClick();
        }

        if (cachedPageUI != null && cachedData != null)
        {
            cachedPageUI.DeleteWorkout(cachedData.id);
        }
    }
}