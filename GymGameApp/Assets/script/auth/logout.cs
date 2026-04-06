using UnityEngine;
using Firebase.Auth; // Manages authentication operations
using UnityEngine.SceneManagement; // For switching between scenes 

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
        
        // Loads the login scene
        SceneManager.LoadScene("auth"); 
    }
}
