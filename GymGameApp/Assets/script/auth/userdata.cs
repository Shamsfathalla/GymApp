using UnityEngine;
using Firebase.Firestore; 

[FirestoreData] // Saves instance of class in database
public class Userdata 
{
    // represents user data stored in database.
    [FirestoreProperty] 
    public string Email { get; set; } // Takes the value of email and saves it in the database
}