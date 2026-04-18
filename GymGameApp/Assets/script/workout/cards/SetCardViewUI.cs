using TMPro; 
using UnityEngine; 

public class SetCardViewUI : MonoBehaviour
{
    [SerializeField] private TMP_Text repsText;
    [SerializeField] private TMP_Text weightText;

    // Updates the UI text with the provided set data
    public void Setup(SetData data)
    {
        repsText.text = data.reps.ToString(); // Convert the integer reps to a string for the text field
        weightText.text = data.weight.ToString(); // Convert the float weight to a string for the text field
    }
}