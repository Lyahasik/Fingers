using System;
using System.Collections.Generic;

using Fingers.Gameplay.Enemies;
using UnityEngine;

namespace Fingers.Core.Services.StaticData
{
    [Serializable]
    public class DifficultyStaticData
    {
        public int transitionScores;
        public float speedMove;
        public float spawnDistance;

        [Space]
        public Sprite background;
        public List<EnemiesGroup> enemiesGroups;
    }
}