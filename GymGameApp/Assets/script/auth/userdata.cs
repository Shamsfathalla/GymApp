using UnityEngine;
using Firebase.Firestore; // For Firestore database operations

[FirestoreData] // Saves instance of class in database
public class Userdata 
{
    // represents user data stored in database.
    [FirestoreProperty] public string Email { get; set; } // Takes the value of email and saves it in the database
}