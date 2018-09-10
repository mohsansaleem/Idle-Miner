using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using pg.im.view;

namespace pg.editor
{
    public static class PGEditorMenu
    {
        [MenuItem("Potato-Games/Idle Miner/Data/CreateDefaultGameState")]
        static void CreateBuildingViewDefinitions()
        {
            DefaultGameState asset = ScriptableObject.CreateInstance<DefaultGameState>();
            AssetDatabase.CreateAsset(asset, DefaultGameState.DefaultGameStateDefinitionPath);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}