using Unity.VisualScripting;
using UnityEngine;

using EmpireCafe.Core.Publish;
using EmpireCafe.Core.Publish.Services.Ads;
using EmpireCafe.Core.Publish.Services.Analytics;
using EmpireCafe.Core.Services;
using EmpireCafe.Core.Services.Factories.Gameplay;
using EmpireCafe.Core.Services.Factories.UI;
using EmpireCafe.Core.Services.GameStateMachine;
using EmpireCafe.Core.Services.GameStateMachine.States;
using EmpireCafe.Core.Services.Progress;
using EmpireCafe.Core.Services.Scene;
using EmpireCafe.Core.Services.StaticData;
using EmpireCafe.Core.Update;
using EmpireCafe.Gameplay.Wallet.Services;
using EmpireCafe.UI.Loading;

namespace EmpireCafe.Core.Initialize
{
    public class InitializerGame : MonoBehaviour
    {
        [SerializeField] private PublishHandler publishHandlerPrefab;
        [SerializeField] private LoadingCurtain curtainPrefab;
        [SerializeField] private UpdateHandler updateHandlerPrefab;

        private ServicesContainer _coreServicesContainer;

        private void Start()
        {
            RegisterServices();
            
            _coreServicesContainer.Single<IGameStateMachine>().Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            _coreServicesContainer = new ServicesContainer();
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
