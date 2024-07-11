using Fingers.Core.Services.StaticData;
using Fingers.UI.Hud;
using Fingers.UI.Information;
using Fingers.UI.MainMenu;

namespace Fingers.Core.Services.Factories.UI
{
    public class UIFactory : Factory, IUIFactory
    {
        private readonly IStaticDataService staticDataService;

        public UIFactory(IStaticDataService staticDataService)
        {
            this.staticDataService = staticDataService;
        }

        public MainMenuHandler CreateMainMenuHandler() => 
            PrefabInstantiate(staticDataService.UI.mainMenuHandlerPrefab);

        public InformationView CreateInformation() => 
            PrefabInstantiate(staticDataService.UI.informationViewPrefab);

        public HudView CreateHUD() => 
            PrefabInstantiate(staticDataService.UI.hudViewPrefab);
    }
}