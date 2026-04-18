using System; 
using System.Collections.Generic; 
using Firebase.Auth; 
using Firebase.Firestore; 
using TMPro; 
using UnityEngine;

public class ExerciseDetailEditPageUI : MonoBehaviour
{
    [SerializeField] private GameObject pagePanel;
    [SerializeField] private TMP_Text exerciseNameText;
    [SerializeField] private Transform setsContent;
    [SerializeField] private GameObject setCardEditPrefab;

    private ExerciseProgressData editingData; 
    private ExerciseDetailPageUI linkedViewPage; 
    private List<SetCardEditUI> activeEditCards = new List<SetCardEditUI>(); // Track the active edit cards for easy data gathering and cleanup

    public void discardButton() 
    { 
        audioManager.instance.PlayClick();

        pagePanel.SetActive(false); 
    }

    public void Open(ExerciseProgressData data, ExerciseDetailPageUI viewPage)
    {
        // Set the current editing data and linked view page for later use when saving
        editingData = data; 
        linkedViewPage = viewPage; 
        exerciseNameText.text = data.exerciseName; 
        pagePanel.SetActive(true);

        activeEditCards.Clear(); 

        foreach (Transform child in setsContent.transform) // Clean up any existing cards before populating new ones
        {
            Destroy(child.gameObject); 
        }

        if (data.currentSets != null)
        {
            foreach (SetData set in data.currentSets) 
            {
                CreateEditCard(set); 
            }
        }
    }

    public void addSetButton()
    {
        audioManager.instance.PlayClick();

        SetData newSet = new SetData();
        newSet.reps = 0;
        newSet.weight = 0;

        CreateEditCard(newSet); 
    }

    private void CreateEditCard(SetData data)
    {
        // When creating an edit card, we also add it to the activeEditCards list so we can easily gather data from them later when saving
        GameObject cardObj = Instantiate(setCardEditPrefab, setsContent); 
        SetCardEditUI cardUI = cardObj.GetComponent<SetCardEditUI>(); 
        
        cardUI.Setup(data, DeleteCard); 
        activeEditCards.Add(cardUI); 
    }

    private void DeleteCard(SetCardEditUI card)
    {
        activeEditCards.Remove(card); 
        Destroy(card.gameObject); 
    }

    public async void saveButton()
    {
        audioManager.instance.PlayClick();

        FirebaseUser currentuser = FirebaseAuth.DefaultInstance.CurrentUser; 

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance; 
        DocumentReference exerciseRef = db.Collection("users").Document(currentuser.UserId).Collection("exerciseProgress").Document(editingData.exerciseName); 

        try 
        {
            // 1. Save the old sets to history directly inside this method
            if (editingData.currentSets != null && editingData.currentSets.Count > 0)
            {
                string dateOnly = DateTime.Now.ToString("dd MMM yyyy"); 
                List<Dictionary<string, object>> firestoreOldSets = new List<Dictionary<string, object>>(); 
                
                foreach (SetData s in editingData.currentSets) // Convert each old set to a dictionary format for Firestore
                {
                    Dictionary<string, object> oldSet = new Dictionary<string, object>();
                    oldSet.Add("reps", s.reps);
                    oldSet.Add("weight", s.weight);

                    firestoreOldSets.Add(oldSet);
                }

                // Create a history entry with the date and old sets, and save it to Firestore under the "history" subcollection of this exercise document
                Dictionary<string, object> historyData = new Dictionary<string, object>();
                historyData.Add("date", dateOnly);
                historyData.Add("sets", firestoreOldSets);
                historyData.Add("savedAt", Timestamp.GetCurrentTimestamp());

                await exerciseRef.Collection("history").AddAsync(historyData); 
                
                if (editingData.history == null) 
                {
                    editingData.history = new List<HistorySession>();        
                }

                List<SetData> oldSetsCopy = new List<SetData>(editingData.currentSets);

                // Create a new HistorySession object and add it to the local editingData history list so that the linked view page can display it 
                HistorySession newSession = new HistorySession();
                newSession.date = dateOnly;
                newSession.sets = oldSetsCopy;

                editingData.history.Insert(0, newSession); 
            }

            // 2. Gather the new sets from the UI cards
            List<SetData> newSets = new List<SetData>();
            List<Dictionary<string, object>> firestoreNewSets = new List<Dictionary<string, object>>();

            // For loop through each active edit card, get the set data and add it to the new sets list
            foreach (SetCardEditUI card in activeEditCards)
            {
                SetData newSet = card.GetSetData();
                newSets.Add(newSet);

                Dictionary<string, object> firestoreSet = new Dictionary<string, object>();
                firestoreSet.Add("reps", newSet.reps);
                firestoreSet.Add("weight", newSet.weight);

                firestoreNewSets.Add(firestoreSet);
            }

            editingData.currentSets = newSets; 

            // 3. Save the new sets to Firebase using basic Dictionary Adds
            Dictionary<string, object> userData = new Dictionary<string, object>();
            userData.Add("exerciseName", editingData.exerciseName);
            userData.Add("currentSets", firestoreNewSets);
            userData.Add("updatedAt", Timestamp.GetCurrentTimestamp());

            await exerciseRef.SetAsync(userData, SetOptions.MergeAll);

            discardButton(); 
            
            if (linkedViewPage != null) 
            {
                linkedViewPage.Open(editingData); 
            }
        }
        catch (Exception error)
        {
            Debug.LogError("Failed to save exercise data: " + error.Message);
        }
    }
}