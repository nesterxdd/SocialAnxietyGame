#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameManagement
{
    public class ApplicationQuit : MonoBehaviour
    {
        public void CloseApplication()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
