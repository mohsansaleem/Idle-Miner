using PG.IdleMiner.Models.DataModels;
using UnityEngine;

namespace PG.IdleMiner.Scenes.Startup
{
    [CreateAssetMenu(menuName = "Idle Miner/Game State")]
    public class DefaultGameState : ScriptableObject
    {
        public static readonly string DefaultGameStateDefinitionPath = "Assets/Resources/DefaultGameState.asset";

        [SerializeField]
        public UserData User;
    }
}