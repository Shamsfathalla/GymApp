using UnityEngine;
using Firebase.Auth; 
using UnityEngine.SceneManagement; 

public class logout : MonoBehaviour
{
    // Signs the user out by clearing their session from Firebase, and goes back to the auth scene
    public void Logout()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        if (auth != null)
        {
            auth.SignOut();
            Debug.Log("User logged out");
        }
        
        audioManager.instance.PlayClick();
        
        // Loads the login scene
        SceneManager.LoadScene("auth"); 
    }
}