using UnityEngine;

using EmpireCafe.Core.Services.StaticData;

namespace EmpireCafe.Core.Services.Factories.Gameplay
{
    public class GameplayFactory : Factory, IGameplayFactory
    {
        private readonly IStaticDataService staticDataService;

        public GameplayFactory(IStaticDataService staticDataService)
        {
            this.staticDataService = staticDataService;
        }

        public Canvas CreateGameplayCanvas() => 
            PrefabInstantiate(staticDataService.UI.gameplayCanvas);
    }
}