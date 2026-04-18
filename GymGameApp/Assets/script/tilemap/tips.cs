using UnityEngine;

public class OpenTips : MonoBehaviour
{
    [SerializeField] private GameObject targetCanvas; 
    
    public void OpenCanvas()
    {
        audioManager.instance.PlayClick();

        targetCanvas.SetActive(true); //activates tips panel
    }

    public void CloseCanvas()
    {
        audioManager.instance.PlayClick();

        targetCanvas.SetActive(false); //deactivates tips panel
    }
}