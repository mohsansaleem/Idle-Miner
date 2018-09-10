using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace pg.editor
{
    public static class PlayFromTheFirstScene
    {
        private const string _playFromFirstMenuStr = "Potato-Games/Always Start From Startup Scene &p";

        static bool PlayFromFirstScene
        {
            get { return EditorPrefs.HasKey(_playFromFirstMenuStr) && EditorPrefs.GetBool(_playFromFirstMenuStr); }
            set { EditorPrefs.SetBool(_playFromFirstMenuStr, value); }
        }

        [MenuItem(_playFromFirstMenuStr, false, 150)]
        static void PlayFromFirstSceneCheckMenu()
        {
            PlayFromFirstScene = !PlayFromFirstScene;
            Menu.SetChecked(_playFromFirstMenuStr, PlayFromFirstScene);

            ShowNotifyOrLog(PlayFromFirstScene ? "Play from Startup scene" : "Play from current scene");
        }

        // The menu won't be gray out, we use this validate method for update check state
        [MenuItem(_playFromFirstMenuStr, true)]
        static bool PlayFromFirstSceneCheckMenuValidate()
        {
            Menu.SetChecked(_playFromFirstMenuStr, PlayFromFirstScene);
            return true;
        }

        // This method is called before any Awake. It's the perfect callback for this feature
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void LoadFirstSceneAtGameBegins()
        {
            if (!PlayFromFirstScene)
                return;

            if (!EditorBuildSettings.scenes.Any())
            {
                Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
                return;
            }

            foreach (GameObject go in Object.FindObjectsOfType<GameObject>())
                go.SetActive(false);

            SceneManager.LoadScene(0);
        }

        static void ShowNotifyOrLog(string msg)
        {
            if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
                EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
            else
                Debug.Log(msg); // When there's no scene view opened, we just print a log
        }
    }
}