using UnityEngine;

namespace Fingers.Core.Services.StaticData
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "Static data/Gameplay")]
    public class GameplayStaticData : ScriptableObject
    {
        public float delayToStartGame;
        public float startSpeedMove;
    }
}