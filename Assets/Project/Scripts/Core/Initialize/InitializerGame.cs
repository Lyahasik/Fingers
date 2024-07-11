using System.Collections;
using Fingers.Core.Publish;
using Fingers.Core.Publish.Services.Ads;
using Fingers.Core.Publish.Services.Analytics;
using Fingers.Core.Services;
using Fingers.Core.Services.Factories.Gameplay;
using Fingers.Core.Services.Factories.UI;
using Fingers.Core.Services.GameStateMachine;
using Fingers.Core.Services.GameStateMachine.States;
using Fingers.Core.Services.Localization;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.Scene;
using Fingers.Core.Services.StaticData;
using Fingers.Core.Update;
using Fingers.Gameplay.Wallet.Services;
using Fingers.UI.Loading;
using Unity.VisualScripting;
using UnityEngine;

namespace Fingers.Core.Initialize
{
    public class InitializerGame : MonoBehaviour
    {
        [SerializeField] private PublishHandler publishHandlerPrefab;
        [SerializeField] private LoadingCurtain curtainPrefab;
        [SerializeField] private UpdateHandler updateHandlerPrefab;

        private ServicesContainer _coreServicesContainer;
        
        private IEnumerator Start()
        {
            var localizationService = new LocalizationService();
            
            yield return localizationService.Initialize();
            
            RegisterServices(localizationService);
            _coreServicesContainer.Single<IGameStateMachine>().Enter<LoadProgressState>();
        }

        private void RegisterServices(LocalizationService localizationService)
        {
            _coreServicesContainer = new ServicesContainer();
            _coreServicesContainer.Register<ILocalizationService>(localizationService);
            
            var gameStateMachine = new GameStateMachine();
            UpdateHandler updateHandler = CreateUpdateHandler();

            RegisterStaticDataService();
            
            PublishHandler publishHandler = CreatePublishHandler();
            RegisterProcessingAnalyticsService();
            RegisterProcessingAdsService();

            RegisterProgressProviderService(gameStateMachine, updateHandler, publishHandler);
            publishHandler.Initialize(_coreServicesContainer.Single<IProgressProviderService>(),
                _coreServicesContainer.Single<IProcessingAdsService>());

            RegisterWalletOperationService(_coreServicesContainer.Single<IProgressProviderService>());
            
            _coreServicesContainer.Register<IUIFactory>(
                new UIFactory(
                    _coreServicesContainer.Single<IStaticDataService>()));
            _coreServicesContainer.Register<IGameplayFactory>(
                new GameplayFactory(
                    _coreServicesContainer.Single<IStaticDataService>()));

            _coreServicesContainer.Register<ISceneProviderService>(
                new SceneProviderService(
                    gameStateMachine,
                    _coreServicesContainer.Single<ILocalizationService>(),
                    updateHandler,
                    _coreServicesContainer.Single<IUIFactory>(),
                    _coreServicesContainer.Single<IGameplayFactory>(),
                    _coreServicesContainer.Single<IStaticDataService>(),
                    _coreServicesContainer.Single<IProcessingAnalyticsService>(),
                    _coreServicesContainer.Single<IProcessingAdsService>(),
                    _coreServicesContainer.Single<IWalletOperationService>(),
                    _coreServicesContainer.Single<IProgressProviderService>()));
            _coreServicesContainer.Single<IProgressProviderService>().SceneProviderService
                = _coreServicesContainer.Single<ISceneProviderService>();
            
            LoadingCurtain curtain = CreateLoadingCurtain();
            GameData gameData = GameDataCreate(curtain, _coreServicesContainer);

            gameStateMachine.Initialize(
                _coreServicesContainer.Single<IProgressProviderService>(),
                gameData.CoroutinesContainer,
                gameData.Curtain);
            _coreServicesContainer.Register<IGameStateMachine>(gameStateMachine);
            
            DontDestroyOnLoad(gameData);
        }

        private void RegisterWalletOperationService(IProgressProviderService progressProviderService)
        {
            var service = new WalletOperationService();
            service.Construct(progressProviderService);
            service.Initialize();
            
            _coreServicesContainer.Register<IWalletOperationService>(service);
        }

        private void RegisterProcessingAnalyticsService()
        {
            var service = new ProcessingAnalyticsService();
            _coreServicesContainer.Register<IProcessingAnalyticsService>(service);
            service.Initialize();
        }

        private void RegisterProcessingAdsService()
        {
            var service = new ProcessingAdsService(_coreServicesContainer.Single<IProcessingAnalyticsService>());
            _coreServicesContainer.Register<IProcessingAdsService>(service);
            service.Initialize();
        }

        private UpdateHandler CreateUpdateHandler() => 
            Instantiate(updateHandlerPrefab);

        private void RegisterStaticDataService()
        {
            var service = new StaticDataService();
            service.Load();
            _coreServicesContainer.Register<IStaticDataService>(service);
        }

        private void RegisterProgressProviderService(GameStateMachine gameStateMachine,
            UpdateHandler updateHandler,
            PublishHandler publishHandler)
        {
            var service = new ProgressProviderService(
                gameStateMachine,
                _coreServicesContainer.Single<IStaticDataService>(),
                publishHandler,
                _coreServicesContainer.Single<IProcessingAnalyticsService>()
                );
            
            service.Initialize(updateHandler);
            
            _coreServicesContainer.Register<IProgressProviderService>(service);
        }

        private PublishHandler CreatePublishHandler()
        {
            PublishHandler publishHandler = Instantiate(publishHandlerPrefab);
            publishHandler.Construct(publishHandlerPrefab.name);
            
            return publishHandler;
        }

        private LoadingCurtain CreateLoadingCurtain()
        {
            LoadingCurtain curtain = Instantiate(curtainPrefab);
            curtain.Construct(curtainPrefab.name,
                _coreServicesContainer.Single<IStaticDataService>().UI);
            
            return curtain;
        }

        private GameData GameDataCreate(LoadingCurtain curtain, ServicesContainer servicesContainer)
        {
            GameData gameData = new GameObject().AddComponent<GameData>();
            gameData.name = nameof(GameData);
            
            Coroutines.CoroutinesContainer coroutinesContainer = gameData.AddComponent<Coroutines.CoroutinesContainer>();
            gameData.Construct(curtain, coroutinesContainer, servicesContainer);
            
            return gameData;
        }
    }
}
