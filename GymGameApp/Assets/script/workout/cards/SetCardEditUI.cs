using TMPro; 
using UnityEngine;
using UnityEngine.UI; 
using System; 

public class SetCardEditUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField repsInput;
    [SerializeField] private TMP_InputField weightInput;
    [SerializeField] private Button deleteButton;

    // The onDelete Action allows the parent UI to know when this card should be removed
    public void Setup(SetData data, Action<SetCardEditUI> onDelete)
    {
        if (data != null) // Only populate the fields if we have valid data
        {
            repsInput.text = data.reps.ToString(); 
            weightInput.text = data.weight.ToString(); 
        }

        deleteButton.onClick.RemoveAllListeners(); // Clear previous listeners to avoid multiple calls
        deleteButton.onClick.AddListener(() => 
        {
            if (onDelete != null) 
            {
                onDelete(this); // Notify the parent UI that this card should be deleted
            }
        }); 
    }

    // This method can be called by the parent UI to get the current values from the input fields
    public SetData GetSetData()
    {
        // 1. Create the variables first
        int reps = 0;
        float weight = 0f;

        // 2. Fill them using TryParse (which is a method that converts a string to a number and returns true if it was successful, false if it failed)
        int.TryParse(repsInput.text, out reps); 
        float.TryParse(weightInput.text, out weight); 

        // 3. Return the result
        return new SetData { reps = reps, weight = weight }; 
    }
}