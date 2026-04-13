using UnityEngine;
using TMPro;

public class ExerciseDetailPageUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pagePanel;
    [SerializeField] private TMP_Text exerciseNameText;
    [SerializeField] private Transform setsContent;

    [SerializeField] private GameObject setCardViewPrefab;
    [SerializeField] private ExerciseDetailEditPageUI editPageUI;
    [SerializeField] private ExerciseHistoryPageUI historyPageUI;

    private ExerciseProgressData currentData; // Holds the current exercise data being displayed

    // Opens the page and displays the exercise data
    public void Open(ExerciseProgressData data)
    {
        currentData = data; // Store the current data for later use (e.g., editing)
        exerciseNameText.text = data.exerciseName; // Set the exercise name text
        pagePanel.SetActive(true); 

        PopulateSets(); // Populate the sets content with the current sets data
    }

    // Spawns the read-only cards for the current sets
    private void PopulateSets()
    {
        setsContent.ClearChildren(); // Clear existing set cards before populating new ones
        if (currentData.currentSets == null) return; // If there are no sets, exit early

        // Loop through each set in the current data and create a card for it
        foreach (SetData set in currentData.currentSets)
        {
            GameObject card = Instantiate(setCardViewPrefab, setsContent); // Create a new card from the prefab and set its parent to the sets content
            
            // Get the SetCardViewUI component from the card and set it up with the current set data
            SetCardViewUI cardUI = card.GetComponent<SetCardViewUI>();
            if (cardUI != null)
            {
                cardUI.Setup(set); // Pass the set data to the card UI to display it
            }
        }
    }

    public void editButton()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        editPageUI.Open(currentData, this);
    }

    public void historyButton()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        historyPageUI.Open(currentData);
    }
    
    public void backButton()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        pagePanel.SetActive(false);
    }
}