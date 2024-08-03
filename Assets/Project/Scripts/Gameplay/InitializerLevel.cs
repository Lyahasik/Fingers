using UnityEngine;

using Fingers.Core.Publish.Services.Ads;
using Fingers.Core.Publish.Services.Analytics;
using Fingers.Core.Services;
using Fingers.Core.Services.Factories.Gameplay;
using Fingers.Core.Services.Factories.UI;
using Fingers.Core.Services.Localization;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;
using Fingers.Gameplay.Movement;
using Fingers.Gameplay.Player;
using Fingers.UI.Gameplay;
using Fingers.UI.Hud;
using Fingers.UI.Information;
using Fingers.UI.Information.Services;
using Fingers.UI.MainMenu;

namespace Fingers.Gameplay
{
    public class InitializerLevel : MonoBehaviour
    {
        private IStaticDataService _staticDataService;
        private ILocalizationService _localizationService;
        private IProgressProviderService _progressProviderService;
        private IProcessingAdsService _processingAdsService;
        private IProcessingAnalyticsService _processingAnalyticsService;
        private IGameplayFactory _gameplayFactory;
        private IUIFactory _uiFactory;

        private ServicesContainer _gameplayServicesContainer;

        public void Construct(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IProgressProviderService progressProviderService,
            IProcessingAdsService processingAdsService,
            IProcessingAnalyticsService processingAnalyticsService,
            IGameplayFactory gameplayFactory,
            IUIFactory uiFactory)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
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

            MainMenuHandler mainMenuHandler = CreateMainMenu();
            CreateGameplay(mainMenuHandler, hudView);
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
        
        private MainMenuHandler CreateMainMenu()
        {
            MainMenuHandler mainMenuHandler = _uiFactory.CreateMainMenuHandler();
            mainMenuHandler.Construct(
                _processingAdsService,
                _gameplayServicesContainer.Single<IInformationService>());
            mainMenuHandler.Initialize(_staticDataService, _localizationService, _progressProviderService);
            
            InformationView information = _uiFactory.CreateInformation();
            information.Initialize(_staticDataService, _processingAdsService);
            
            _gameplayServicesContainer.Single<IInformationService>().Initialize(information);

            return mainMenuHandler;
        }

        private void CreateGameplay(MainMenuHandler mainMenuHandler, HudView hudView)
        {
            EnemiesArea enemiesArea = _gameplayFactory.CreateEnemiesArea();
            enemiesArea.Construct(_staticDataService.Gameplay, _gameplayFactory);
            enemiesArea.Initialize();
            
            PlayerFinger playerFinger = _gameplayFactory.CreatePlayerFinger();
            
            GameplayHandler gameplayHandler = _gameplayFactory.CreateGameplayHandler();
            gameplayHandler.Construct(_progressProviderService, mainMenuHandler, hudView);
            gameplayHandler.Initialize(_staticDataService, enemiesArea, playerFinger);

            mainMenuHandler.SetGameplayHandler(gameplayHandler);
            
            hudView.Initialize();
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