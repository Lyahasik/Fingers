using EmpireCafe.UI.Hud;
using EmpireCafe.UI.Information;
using EmpireCafe.UI.MainMenu;

namespace EmpireCafe.Core.Services.Factories.UI
{
    public interface IUIFactory : IService
    {
        public MainMenuHandler CreateMainMenu();
        public InformationView CreateInformation();
        public HudView CreateHUD();
    }
}