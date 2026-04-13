using System; 
using System.Collections.Generic; 
using Firebase.Auth; 
using Firebase.Firestore; 
using Firebase.Extensions; 
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
    private List<SetCardEditUI> activeEditCards = new List<SetCardEditUI>(); // We keep track of the active edit cards so we can gather their data when saving and also delete them when needed

    public void discardButton() 
    { 
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        pagePanel.SetActive(false); 
    }

    public void Open(ExerciseProgressData data, ExerciseDetailPageUI viewPage)
    {
        editingData = data; 
        linkedViewPage = viewPage; 
        exerciseNameText.text = data.exerciseName; 
        pagePanel.SetActive(true);

        activeEditCards.Clear(); 
        setsContent.ClearChildren(); 

        // We loop through the current sets in the provided data and create an edit card for each one, pre-populating it with the existing reps and weight. This allows the user to see their current progress and make adjustments as needed.
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
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        CreateEditCard(new SetData { reps = 0, weight = 0 }); 
    }

    private void CreateEditCard(SetData data)
    {
        // Create the edit card prefab and set it up with the provided data
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

    public void saveButton()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();

        var currentuser = FirebaseAuth.DefaultInstance.CurrentUser; 
        if (currentuser == null) return;

        var db = FirebaseFirestore.DefaultInstance; 
        // We get a reference to the specific exercise document for the current user. This is where we'll save the updated sets data.
        var exerciseRef = db.Collection("users").Document(currentuser.UserId).Collection("exerciseProgress").Document(editingData.exerciseName); 

        // 1. Save the old sets to history first (if we have any)
        if (editingData.currentSets != null && editingData.currentSets.Count > 0)
        {
            SaveHistory(exerciseRef);
        }

        // 2. Gather the new sets from the UI cards
        List<SetData> newSets = new List<SetData>();
        List<Dictionary<string, object>> firestoreNewSets = new List<Dictionary<string, object>>();

        foreach (SetCardEditUI card in activeEditCards)
        {
            SetData newSet = card.GetSetData();
            newSets.Add(newSet);
            firestoreNewSets.Add(new Dictionary<string, object> { { "reps", newSet.reps }, { "weight", newSet.weight } });
        }

        editingData.currentSets = newSets; 

        // 3. Save the new sets to Firebase
        var user = new Dictionary<string, object>
        {
            { "exerciseName", editingData.exerciseName }, 
            { "currentSets", firestoreNewSets }, 
            { "updatedAt", Timestamp.GetCurrentTimestamp() }
        };

        // If it's a new document (which shouldn't be the case here since we're editing), we also want to set createdAt
        exerciseRef.SetAsync(user, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled) return; 
            
            discardButton(); // Re-using your discard method to close the panel!
            if (linkedViewPage != null) linkedViewPage.Open(editingData); 
        });
    }

    // This method saves the current sets to the history collection before we overwrite them with new data
    private void SaveHistory(DocumentReference exerciseRef)
    {
        string dateOnly = DateTime.Now.ToString("dd MMM yyyy"); 
        List<Dictionary<string, object>> firestoreOldSets = new List<Dictionary<string, object>>(); // We create a list of dictionaries to store the old sets in a format suitable for Firestore
        
        // We just loop through the currentSets directly instead of making an "oldSets" list
        foreach (SetData s in editingData.currentSets) 
        {
            firestoreOldSets.Add(new Dictionary<string, object> { { "reps", s.reps }, { "weight", s.weight } });
        }

        exerciseRef.Collection("history").AddAsync(new Dictionary<string, object> {
            { "date", dateOnly },
            { "sets", firestoreOldSets }, 
            { "savedAt", Timestamp.GetCurrentTimestamp() }
        }); 
        
        // We also want to update our local data's history so that if the user opens the history page after saving, they can see the new entry without needing to fetch from Firestore again
        if (editingData.history == null) editingData.history = new List<HistorySession>();        
        // Make a copy of the list for the local history
        List<SetData> oldSetsCopy = new List<SetData>(editingData.currentSets);
        editingData.history.Insert(0, new HistorySession { date = dateOnly, sets = oldSetsCopy }); 
    }
}