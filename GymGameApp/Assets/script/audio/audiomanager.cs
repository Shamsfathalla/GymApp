using UnityEngine;

public class audioManager : MonoBehaviour
{
    public static audioManager instance; // Singleton instance to be accessed globally
    
    [Header("Audio Sources")]
    public AudioSource backgroundMusic;
    public AudioSource clickSound; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make this object persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
            return;
        }
    }

    public void PlayClick()
    {
        if (clickSound != null) // Check if the click sound and audio source are assigned
        {
            clickSound.PlayOneShot(clickSound.clip); // Play the click sound without interruption
        }
    }
}