using Fingers.Core.Services.StaticData;
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
    }
}