using UnityEngine;

using EmpireCafe.Core.Publish.Services.Ads;
using EmpireCafe.Core.Publish.Services.Analytics;
using EmpireCafe.Core.Services;
using EmpireCafe.Core.Services.Factories.Gameplay;
using EmpireCafe.Core.Services.Factories.UI;
using EmpireCafe.Core.Services.Progress;
using EmpireCafe.Core.Services.StaticData;
using EmpireCafe.UI.Hud;
using EmpireCafe.UI.Information;
using EmpireCafe.UI.Information.Services;
using EmpireCafe.UI.MainMenu;

namespace EmpireCafe.Gameplay
{
    public class InitializerLevel : MonoBehaviour
    {
        private IStaticDataService _staticDataService;
        private IProgressProviderService _progressProviderService;
        private IProcessingAdsService _processingAdsService;
        private IProcessingAnalyticsService _processingAnalyticsService;
        private IGameplayFactory _gameplayFactory;
        private IUIFactory _uiFactory;

        private ServicesContainer _gameplayServicesContainer;

        public void Construct(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService,
            IProcessingAdsService processingAdsService,
            IProcessingAnalyticsService processingAnalyticsService,
            IGameplayFactory gameplayFactory,
            IUIFactory uiFactory)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
            _processingAdsService = processingAdsService;
            _processingAnalyticsService = processingAnalyticsService;
            _gameplayFactory = gameplayFactory;
            _uiFactory = uiFactory;
        }

        public void Initialize()
        {
            RegisterGameplayServices();
            
            HudView hudView = CreateHUD();

            CreateMainMenu();
            CreateGameplay(hudView);
        }

        private void OnDestroy()
        {
            ClearGameplayServices();
        }

        private void RegisterGameplayServices()
        {
            _gameplayServicesContainer = new ServicesContainer();
            
            _gameplayServicesContainer.Register<IInformationService>(new InformationService());
        }
        
        private void CreateMainMenu()
        {
            MainMenuHandler mainMenu = _uiFactory.CreateMainMenu();
            mainMenu.Construct(
                _uiFactory,
                _processingAdsService,
                _gameplayServicesContainer.Single<IInformationService>());
            mainMenu.Initialize();
            
            InformationView information = _uiFactory.CreateInformation();
            information.Initialize(_staticDataService, _processingAdsService);
            
            _gameplayServicesContainer.Single<IInformationService>().Initialize(information);
        }

        private void CreateGameplay(HudView hudView)
        {
            Canvas gameplayCanvas = _gameplayFactory.CreateGameplayCanvas();
            
            hudView.Initialize(_staticDataService, _processingAdsService, _progressProviderService);
        }

        private HudView CreateHUD()
        {
            HudView hudView = _uiFactory.CreateHUD();

            return hudView;
        }

        private void ClearGameplayServices()
        {
            _gameplayServicesContainer.Clear();
            
            _gameplayServicesContainer = null;
        }
    }
}