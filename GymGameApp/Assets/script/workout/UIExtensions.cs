using UnityEngine;

public static class UIExtensions
{
    // A custom shortcut to instantly destroy all child objects inside a UI panel (Exercise Cards, History Logs, etc.)
    public static void ClearChildren(this Transform parent)
    {
        foreach (Transform child in parent) // Loop through each child transform of the parent
        {
            Object.Destroy(child.gameObject); 
        }
    }
}