using UnityEngine;

public class OpenTips : MonoBehaviour
{
    public GameObject targetCanvas; 
    
    public void OpenCanvas()
    {
            targetCanvas.SetActive(true); //activates tips panel
    }

    public void CloseCanvas()
    {
            targetCanvas.SetActive(false); //deactivates tips panel
    }
}