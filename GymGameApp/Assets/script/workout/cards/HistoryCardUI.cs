using TMPro;
using UnityEngine;

public class HistoryCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text repsText;
    [SerializeField] private TMP_Text weightText;

    // Fills the UI text fields with the historical set data
    public void Setup(string date, SetData setData)
    {
        dateText.text = date; // Display the date of the workout
        repsText.text = setData.reps.ToString(); // Display the number of repetitions performed
        weightText.text = setData.weight.ToString(); // Display the weight lifted in the set
    }
}