using UnityEditor;
using UnityEngine;

public class EditorUtilities : MonoBehaviour
{
    [MenuItem("Reset/Reset Prefs")]
    private static void Reset()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
    }
}
