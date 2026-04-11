using TMPro; 
using UnityEngine; 
using UnityEngine.UI;

public class WorkoutCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text workoutNameText;
    [SerializeField] private Button accessButton;
    [SerializeField] private Button deleteButton;

    public void Setup(WorkoutData data, WorkoutsPageUI pageUI)
    {
        // Set workout name
        if (data != null && !string.IsNullOrEmpty(data.name))
        {
            workoutNameText.text = data.name;
        }
        else
        {
            workoutNameText.text = "Unnamed Workout";
        }

        accessButton.onClick.RemoveAllListeners(); // Clear previous listeners to avoid multiple calls
        accessButton.onClick.AddListener(() => 
        {
            // Play audio
            if (audioManager.instance != null)
            {
                audioManager.instance.PlayClick();
            }
            pageUI.OpenWorkoutDetails(data);
        });

        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(() => 
        {
            // Play audio
            if (audioManager.instance != null)
            {
                audioManager.instance.PlayClick();
            }
            pageUI.DeleteWorkout(data.id);
        });
    }
}