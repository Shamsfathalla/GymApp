using System; 
using TMPro; 
using UnityEngine;
using UnityEngine.UI; 

public class WorkoutExerciseCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text exerciseNameText;
    [SerializeField] private Button openExerciseButton;

    // Cache the variables 
    private Action<string> cachedClickAction;
    private string cachedExerciseName;

    public void Setup(string exerciseName, Action<string> onClickAction)
    {
        exerciseNameText.text = exerciseName; 

        cachedExerciseName = exerciseName; // Cache the exercise name for later use
        cachedClickAction = onClickAction; // Cache the click action for later use

        openExerciseButton.onClick.RemoveAllListeners(); 
        
        // Add listener using a standard private method
        openExerciseButton.onClick.AddListener(OnButtonClicked); 
    }

    private void OnButtonClicked()
    {
        if (audioManager.instance != null)
        {
            audioManager.instance.PlayClick();
        }

        if (cachedClickAction != null)
        {
            cachedClickAction(cachedExerciseName); 
        }
    }
}