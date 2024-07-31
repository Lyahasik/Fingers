using UnityEngine;

using Fingers.Core.Services.StaticData;
using Fingers.Gameplay.Enemies;
using Fingers.Gameplay.Movement;
using Fingers.UI.Gameplay;

namespace Fingers.Core.Services.Factories.Gameplay
{
    public class GameplayFactory : Factory, IGameplayFactory
    {
        private readonly IStaticDataService staticDataService;

        public GameplayFactory(IStaticDataService staticDataService)
        {
            this.staticDataService = staticDataService;
        }

        public GameplayHandler CreateGameplayHandler() => 
            PrefabInstantiate(staticDataService.UI.gameplayHandler);

        public EnemiesArea CreateEnemiesArea() => 
            PrefabInstantiate(staticDataService.Gameplay.enemiesArea);
        
        public EnemiesGroup CreateEnemiesGroup(EnemiesGroup enemiesGroup, Transform parent) =>
            PrefabInstantiate(enemiesGroup, parent);
    }
}