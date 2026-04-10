using UnityEngine;
using Firebase; // Core Firebase functionalities
using Firebase.Auth; // Manages authentication operations
using Firebase.Extensions; // Ensures Firebase tasks run safely on Unity's main thread
using Firebase.Firestore; // For Firestore database operations
using TMPro; 
using UnityEngine.SceneManagement; 

public class authentication : MonoBehaviour
{
    private FirebaseAuth _auth; // Manages user authentication
    private bool _firebaseReady = false; // Indicates if Firebase is initialized and ready to use
    private FirebaseFirestore _db; // Firestore database instance for storing user data

    [Header("Login Fields")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;

    [Header("Signup Fields")]
    public TMP_InputField signupEmail;
    public TMP_InputField signupPassword;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Initialize Firebase
                _auth = FirebaseAuth.DefaultInstance;
                _firebaseReady = true;
                _db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase is ready ");
            }
            else
            {
                Debug.LogError("Firebase not ready: " + task.Result);
            }
        });
    }

    // If the operation fails, an error is logged 
    // If successful, the user’s email is retrieved and confirmed
    public void SignUp()
    {
        _auth.CreateUserWithEmailAndPasswordAsync(
            signupEmail.text.Trim(),
            signupPassword.text.Trim()
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Signup Failed");
                return;
            }
            
            // Get the successfully created user
            Firebase.Auth.FirebaseUser newUser = task.Result.User;

            // Save user data
            Userdata userData = new Userdata
            {
                Email = newUser.Email
            };

            _db.Collection("users").Document(newUser.UserId).SetAsync(userData).ContinueWithOnMainThread(dbTask =>
            {
                if (dbTask.IsFaulted)
                {
                    Debug.LogError("Failed to save user data: " + dbTask.Exception);
                }
                else
                {
                    Debug.Log("User data saved successfully");
                }
            });

            Debug.Log("Signup Successful: " + task.Result.User.Email);
            SceneManager.LoadScene("gym"); // Loads the main scene after successful login
        });
    }

    // Failure indicates incorrect credentials or connection issues 
    // Success confirms authentication and retrieves user information
    public void Login()
    {
        _auth.SignInWithEmailAndPasswordAsync(
            loginEmail.text.Trim(),
            loginPassword.text.Trim()
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Login Failed: " + task.Exception);
                return;
            }

            Debug.Log("Login Successful");
            SceneManager.LoadScene("gym"); // Loads the main scene after successful login
        });
    }
}