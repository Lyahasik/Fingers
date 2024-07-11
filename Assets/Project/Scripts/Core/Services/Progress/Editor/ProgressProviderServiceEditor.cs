using UnityEditor;
using UnityEngine;

namespace Core.Services.Progress.Editor
{
    public class ProgressProviderServiceEditor
    {
        [MenuItem("MyLogic/Progress/Reset")]
        public static void Reset()
        {
            Debug.Log($"[ProgressProviderService] Reset data.");
            PlayerPrefs.DeleteAll();
        }
    }
}