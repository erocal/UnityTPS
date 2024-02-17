using UnityEditor;
using UnityEngine;

public class QuitGame : MonoBehaviour
{

    #region -- onClick --

    public void onQuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();
        }
#endif
    }

    #endregion

}
