using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExerciseHistoryPageUI : MonoBehaviour
{
    [SerializeField] private GameObject pagePanel;
    [SerializeField] private TMP_Text exerciseNameText;
    [SerializeField] private Transform historyContent;
    [SerializeField] private GameObject historyCardPrefab;

    public void Open(ExerciseProgressData data) // Method to open the history page and populate it with data
    {
        pagePanel.SetActive(true);
        exerciseNameText.text = $"{data.exerciseName} History"; 

        PopulateHistory(data.history); 
    }

    // Clears old entries and spawns new history cards
    private void PopulateHistory(List<HistorySession> history)
    {
        historyContent.ClearChildren(); 

        if (history == null) 
        {
            return; 
        }

        // A nested loop to go through each session, and then each set inside that session
        foreach (HistorySession session in history)
        {
            foreach (SetData singleSet in session.sets) // Loop through each set in the session
            {
                GameObject card = Instantiate(historyCardPrefab, historyContent); // Create a new card for each set
                HistoryCardUI cardUI = card.GetComponent<HistoryCardUI>(); // Get the HistoryCardUI component to set up the card's display
                
                if (cardUI != null)
                {
                    cardUI.Setup(session.date, singleSet);
                }
            }
        }
    }

    public void backButton()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        pagePanel.SetActive(false);
    }
}