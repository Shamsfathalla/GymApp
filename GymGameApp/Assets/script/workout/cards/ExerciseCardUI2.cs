using TMPro;
using UnityEngine; 
using UnityEngine.UI; 

public class ExerciseCardUI2 : MonoBehaviour
{
    [SerializeField] private TMP_Text exerciseNameText;
    [SerializeField] private Button actionButton;

    public void Setup(ExerciseData data, WorkoutBuilderUI builder, bool isSelected) 
    {
        // Set exercise name
        if (data != null && !string.IsNullOrEmpty(data.name))
        {
            exerciseNameText.text = data.name;
        }
        else
        {
            exerciseNameText.text = "";
        }

        actionButton.onClick.RemoveAllListeners(); // Clear previous listeners to avoid multiple calls
        actionButton.onClick.AddListener(() => 
        {
            // Play audio
            if (audioManager.instance != null)
            {
                audioManager.instance.PlayClick();
            }

            if (isSelected) // If already selected, remove it from the workout
            {
                builder.RemoveExercise(data.name); 
            }
            else 
            {
                builder.AddExercise(data); 
            }
        });
    }
}