using TMPro; 
using UnityEngine;
using UnityEngine.UI; 
using System; 

public class SetCardEditUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField repsInput;
    [SerializeField] private TMP_InputField weightInput;
    [SerializeField] private Button deleteButton;

    private Action<SetCardEditUI> cachedDeleteAction;

    // The onDelete Action allows the parent UI to know when this card should be removed
    public void Setup(SetData data, Action<SetCardEditUI> onDelete)
    {
        if (data != null) 
        {
            repsInput.text = data.reps.ToString(); 
            weightInput.text = data.weight.ToString(); 
        }

        cachedDeleteAction = onDelete;

        deleteButton.onClick.RemoveAllListeners(); 
        deleteButton.onClick.AddListener(OnDeleteClicked); 
    }

    private void OnDeleteClicked()
    {
        if (audioManager.instance != null)
        {
            audioManager.instance.PlayClick();
        }
        
        if (cachedDeleteAction != null) 
        {
            cachedDeleteAction(this); 
        }
    }

    // This method can be called by the parent UI to get the current values from the input fields
    public SetData GetSetData()
    {
        int reps = 0;
        float weight = 0f;

        int.TryParse(repsInput.text, out reps); // Try to parse the reps input as an integer
        float.TryParse(weightInput.text, out weight);  // Try to parse the weight input as a float

        // Create a new SetData object with the parsed values
        SetData newData = new SetData();
        newData.reps = reps;
        newData.weight = weight;

        return newData; 
    }
}