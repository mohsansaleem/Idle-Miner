using PG.IdleMiner.Contexts.Startup;
using UnityEngine;
using UnityEditor;

namespace PG.Editor
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