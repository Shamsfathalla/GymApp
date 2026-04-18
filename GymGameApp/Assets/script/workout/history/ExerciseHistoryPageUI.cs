using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExerciseHistoryPageUI : MonoBehaviour
{
    [SerializeField] private GameObject pagePanel;
    [SerializeField] private TMP_Text exerciseNameText;
    [SerializeField] private Transform historyContent;
    [SerializeField] private GameObject historyCardPrefab;

    public void Open(ExerciseProgressData data) 
    {
        pagePanel.SetActive(true);

        // Set the title of the page to include the exercise name
        exerciseNameText.text = data.exerciseName + " History"; 

        PopulateHistory(data.history); 
    }

    // Clears old entries and spawns new history cards
    private void PopulateHistory(List<HistorySession> history)
    {
        foreach (Transform child in historyContent.transform) 
        {
            Destroy(child.gameObject); 
        }

        if (history == null) 
        {
            return; 
        }

        // A nested loop to go through each session, and then each set inside that session
        foreach (HistorySession session in history)
        {
            foreach (SetData singleSet in session.sets) 
            {
                GameObject card = Instantiate(historyCardPrefab, historyContent); 
                HistoryCardUI cardUI = card.GetComponent<HistoryCardUI>(); 
                
                if (cardUI != null)
                {
                    cardUI.Setup(session.date, singleSet);
                }
            }
        }
    }

    public void backButton()
    {
        audioManager.instance.PlayClick();

        pagePanel.SetActive(false);
    }
}