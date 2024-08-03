using UnityEngine;

using Fingers.Gameplay.Enemies;
using Fingers.Gameplay.Movement;
using Fingers.Gameplay.Player;
using Fingers.UI.Gameplay;

namespace Fingers.Core.Services.Factories.Gameplay
{
    public interface IGameplayFactory : IService
    {
        public GameplayHandler CreateGameplayHandler();
        public EnemiesArea CreateEnemiesArea();
        public PlayerFinger CreatePlayerFinger();
        public EnemiesGroup CreateEnemiesGroup(EnemiesGroup enemiesGroup, Transform parent);
    }
}