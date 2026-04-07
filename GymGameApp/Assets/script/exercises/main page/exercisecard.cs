using UnityEngine;
using TMPro; // For TextMeshPro UI elements
using UnityEngine.UI; // For using UI components like Button

public class exerciseCard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI exerciseNameText;
    [SerializeField] private Button viewButton;

    private string documentId;

    public void Setup(string exerciseName, string id)
    {
        exerciseNameText.text = exerciseName;
        documentId = id;

        viewButton.onClick.AddListener(Details);
    }

    private void Details()
    {
        Debug.Log("Load details for document ID: " + documentId);
        
        exercisedetails.Instance.LoadExercise(documentId); // Method to load exercise details using the document ID
    }
}