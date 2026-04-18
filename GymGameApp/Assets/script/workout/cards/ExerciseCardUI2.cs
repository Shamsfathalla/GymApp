using TMPro;
using UnityEngine; 
using UnityEngine.UI; 

public class ExerciseCardUI2 : MonoBehaviour
{
    [SerializeField] private TMP_Text exerciseNameText;
    [SerializeField] private Button actionButton;

    // Cache the data and builder references for use in the button click handler
    private ExerciseData cachedData;
    private WorkoutBuilderUI cachedBuilder;
    private bool cachedIsSelected;

    public void Setup(ExerciseData data, WorkoutBuilderUI builder, bool isSelected) 
    {
        cachedData = data;
        cachedBuilder = builder;
        cachedIsSelected = isSelected;

        // Set exercise name using standard, expanded checks
        if (data != null && data.name != null && data.name != "")
        {
            exerciseNameText.text = data.name;
        }
        else
        {
            exerciseNameText.text = "";
        }

        actionButton.onClick.RemoveAllListeners(); 
        
        // Add listener using a standard private method
        actionButton.onClick.AddListener(OnActionClicked);
    }

    private void OnActionClicked()
    {
        if (audioManager.instance != null)
        {
            audioManager.instance.PlayClick();
        }

        if (cachedIsSelected == true) 
        {
            cachedBuilder.RemoveExercise(cachedData.name); 
        }
        else 
        {
            cachedBuilder.AddExercise(cachedData); 
        }
    }
}