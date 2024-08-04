using System.Collections.Generic;
using UnityEngine;

using Fingers.Gameplay.Movement;
using Fingers.Gameplay.Player;

namespace Fingers.Core.Services.StaticData
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "Static data/Gameplay")]
    public class GameplayStaticData : ScriptableObject
    {
        public float delayToStartGame;
        public float playerRadius = 1f;

        [Space]
        public EnemiesArea enemiesArea;
        public PlayerFinger playerFinger;
        public List<DifficultyStaticData> difficulties;
    }
}