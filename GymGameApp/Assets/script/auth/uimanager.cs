using UnityEngine;

public class uimanager : MonoBehaviour
{
    public GameObject loginCanvas;
    public GameObject signupCanvas;

    // go to sign up canvas from login panel
    public void GoToSignup()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();

        loginCanvas.SetActive(false); //deactivates login panel
        signupCanvas.SetActive(true); //activates signup panel
    }

    // go back to login canvas from sign up panel
    public void GoToLogin()
    {
        if (audioManager.instance != null) audioManager.instance.PlayClick();
        
        signupCanvas.SetActive(false); //deactivates signup panel
        loginCanvas.SetActive(true); //activates login panel
    }
}