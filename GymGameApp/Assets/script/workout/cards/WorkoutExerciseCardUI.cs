using System; 
using TMPro; 
using UnityEngine;
using UnityEngine.UI; 

public class WorkoutExerciseCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text exerciseNameText;
    [SerializeField] private Button openExerciseButton;

    public void Setup(string exerciseName, Action<string> onClickAction)
    {
        exerciseNameText.text = exerciseName; // Set exercise name

        openExerciseButton.onClick.RemoveAllListeners(); // Clear previous listeners to avoid multiple calls
        
        openExerciseButton.onClick.AddListener(() => 
        {
            // Play audio
            if (audioManager.instance != null)
            {
                audioManager.instance.PlayClick();
            }

            // Standard check to make sure the Action isn't empty before calling it
            if (onClickAction != null)
            {
                onClickAction(exerciseName); // Call the provided Action with the exercise name
            }
        }); 
    }
}