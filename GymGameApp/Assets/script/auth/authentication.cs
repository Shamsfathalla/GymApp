using System;
using UnityEngine;
using Firebase; 
using Firebase.Auth; 
using Firebase.Firestore; 
using TMPro; 
using UnityEngine.SceneManagement; 

public class authentication : MonoBehaviour
{
    private FirebaseAuth auth; 
    private FirebaseFirestore db; 

    [SerializeField] private TMP_InputField loginEmail;
    [SerializeField] private TMP_InputField loginPassword;

    [SerializeField] private TMP_InputField signupEmail;
    [SerializeField] private TMP_InputField signupPassword;

    // 1. Add a reference for your error text
    [SerializeField] private TMP_Text errorText; 

    async void Start() 
    {
        DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;
            Debug.Log("Firebase is ready");

            if (auth.CurrentUser != null)
            {
                Debug.Log("User already signed in");
                SceneManager.LoadScene("gym"); 
            }
        }
        else
        {
            Debug.Log("Firebase not ready");
        }
    }

    public async void SignUp() 
    {
        audioManager.instance.PlayClick();
        errorText.text = ""; // 2. Clear out any old error messages when a new attempt starts
        
        try 
        {
            await auth.CreateUserWithEmailAndPasswordAsync(signupEmail.text, signupPassword.text);
            
            FirebaseUser newUser = auth.CurrentUser; 

            Userdata userData = new Userdata(); 
            userData.Email = newUser.Email; 

            await db.Collection("users").Document(newUser.UserId).SetAsync(userData);
            
            Debug.Log("User data saved successfully");
            SceneManager.LoadScene("gym"); 
        }
        catch (Exception error)
        {
            // 3. Display the error to the player
            errorText.color = Color.red;
            errorText.text = "Signup Failed: " + error.Message; 
            Debug.Log("Signup Failed: " + error.Message);
        }
    }

    public async void Login() 
    {
        audioManager.instance.PlayClick();
        errorText.text = ""; // Clear out any old error messages
        
        try
        {
            await auth.SignInWithEmailAndPasswordAsync(loginEmail.text, loginPassword.text);
            
            Debug.Log("Login Successful");
            SceneManager.LoadScene("gym"); 
        }
        catch (Exception error)
        {
            // Display the error to the player
            errorText.color = Color.red;
            errorText.text = "Login Failed: " + error.Message; 
            Debug.Log("Login Failed: " + error.Message);
        }
    }
}