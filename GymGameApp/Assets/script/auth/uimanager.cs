using UnityEngine;

public class uimanager : MonoBehaviour
{
    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject signupCanvas;

    // go to sign up canvas from login panel
    public void GoToSignup()
    {
        audioManager.instance.PlayClick();

        loginCanvas.SetActive(false); //deactivates login panel
        signupCanvas.SetActive(true); //activates signup panel
    }

    // go back to login canvas from sign up panel
    public void GoToLogin()
    {
        audioManager.instance.PlayClick();
        
        signupCanvas.SetActive(false); //deactivates signup panel
        loginCanvas.SetActive(true); //activates login panel
    }
}