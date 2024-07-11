using Fingers.UI.Hud;
using Fingers.UI.Information;
using Fingers.UI.MainMenu;

namespace Fingers.Core.Services.Factories.UI
{
    public interface IUIFactory : IService
    {
        public MainMenuHandler CreateMainMenuHandler();
        public InformationView CreateInformation();
        public HudView CreateHUD();
    }
}