using System.Collections.Generic;
using UnityEngine;

using Fingers.Gameplay.Movement;

namespace Fingers.Core.Services.StaticData
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "Static data/Gameplay")]
    public class GameplayStaticData : ScriptableObject
    {
        public float delayToStartGame;
        public float startSpeedMove;

        [Space]
        public float spawnDistance;
        public EnemiesArea enemiesArea;
        public List<LevelStaticData> Levels;
    }
}