using pg.im.model.data;
using UnityEngine;

namespace pg.im.view
{
    public class DefaultGameState : ScriptableObject
    {
        public static readonly string DefaultGameStateDefinitionPath = "Assets/Resources/DefaultGameState.asset";

        [SerializeField]
        public UserData User;
    }
}