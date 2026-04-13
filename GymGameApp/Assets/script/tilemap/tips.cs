using UnityEngine;

public class OpenTips : MonoBehaviour
{
    public GameObject targetCanvas; 
    
    public void OpenCanvas()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        targetCanvas.SetActive(true); //activates tips panel
    }

    public void CloseCanvas()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        targetCanvas.SetActive(false); //deactivates tips panel
    }
}