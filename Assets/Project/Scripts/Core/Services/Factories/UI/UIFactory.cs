using EmpireCafe.Core.Services.StaticData;
using EmpireCafe.UI.Hud;
using EmpireCafe.UI.Information;
using EmpireCafe.UI.MainMenu;

namespace EmpireCafe.Core.Services.Factories.UI
{
    public class UIFactory : Factory, IUIFactory
    {
        private readonly IStaticDataService staticDataService;

        public UIFactory(IStaticDataService staticDataService)
        {
            this.staticDataService = staticDataService;
        }

        public MainMenuHandler CreateMainMenu() => 
            PrefabInstantiate(staticDataService.UI.mainMenuHandlerPrefab);

        public InformationView CreateInformation() => 
            PrefabInstantiate(staticDataService.UI.informationViewPrefab);

        public HudView CreateHUD() => 
            PrefabInstantiate(staticDataService.UI.hudViewPrefab);
    }
}