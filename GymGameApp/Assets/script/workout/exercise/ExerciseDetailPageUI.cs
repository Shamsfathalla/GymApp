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

    private ExerciseProgressData currentData; 

    // Opens the page and displays the exercise data
    public void Open(ExerciseProgressData data)
    {
        currentData = data; 
        exerciseNameText.text = data.exerciseName; 
        pagePanel.SetActive(true); 

        PopulateSets(); 
    }

    // Spawns the read-only cards for the current sets
    private void PopulateSets()
    {
        foreach (Transform child in setsContent.transform) 
        {
            Destroy(child.gameObject); 
        }

        // Expanded the early exit to look like a standard if statement
        if (currentData.currentSets == null) 
        {
            return; 
        }

        // Loop through each set in the current data and create a card for it
        foreach (SetData set in currentData.currentSets)
        {
            GameObject card = Instantiate(setCardViewPrefab, setsContent); 
            
            SetCardViewUI cardUI = card.GetComponent<SetCardViewUI>();
            
            if (cardUI != null)
            {
                cardUI.Setup(set); 
            }
        }
    }

    public void editButton()
    {
        audioManager.instance.PlayClick();
        
        editPageUI.Open(currentData, this);
    }

    public void historyButton()
    {
        audioManager.instance.PlayClick();

        historyPageUI.Open(currentData);
    }
    
    public void backButton()
    {
        audioManager.instance.PlayClick();

        pagePanel.SetActive(false);
    }
}