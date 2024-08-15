using System;
using Fingers.Constants;
using Fingers.Core.Publish.Services.Ads;
using Fingers.Core.Publish.Services.Analytics;
using Fingers.Core.Services.Factories.Gameplay;
using Fingers.Core.Services.Factories.UI;
using Fingers.Core.Services.GameStateMachine;
using Fingers.Core.Services.GameStateMachine.States;
using Fingers.Core.Services.Localization;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;
using Fingers.Core.Update;
using Fingers.Gameplay;
using Fingers.Gameplay.Wallet.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fingers.Core.Services.Scene
{
    public class SceneProviderService : ISceneProviderService
    {
        private readonly IGameStateMachine gameStateMachine;
        private readonly IStaticDataService staticDataService;
        private readonly ILocalizationService localizationService;
        private readonly UpdateHandler updateHandler;
        private readonly IUIFactory uiFactory;
        private readonly IGameplayFactory gameplayFactory;
        private readonly IProcessingAnalyticsService processingAnalyticsService;
        private readonly IProcessingAdsService processingAdsService;
        private readonly IProgressProviderService progressProviderService;
        private readonly IWalletOperationService walletOperationService;

        private string _nameNewActiveScene;

        private int _currentDungeonId;
        private int _currentLevelId;

        public SceneProviderService(IGameStateMachine gameStateMachine,
            ILocalizationService localizationService,
            UpdateHandler updateHandler,
            IUIFactory uiFactory,
            IGameplayFactory gameplayFactory,
            IStaticDataService staticDataService,
            IProcessingAnalyticsService processingAnalyticsService,
            IProcessingAdsService processingAdsService,
            IWalletOperationService walletOperationService,
            IProgressProviderService progressProviderService)
        {
            this.gameStateMachine = gameStateMachine;
            this.localizationService = localizationService;
            this.updateHandler = updateHandler;
            this.uiFactory = uiFactory;
            this.gameplayFactory = gameplayFactory;
            this.staticDataService = staticDataService;
            this.processingAnalyticsService = processingAnalyticsService;
            this.processingAdsService = processingAdsService;
            this.walletOperationService = walletOperationService;
            this.progressProviderService = progressProviderService;
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void LoadLevelScene()
        {
            Debug.Log("Current active scene : " + SceneManager.GetActiveScene().name);
            gameStateMachine.Enter<LoadSceneState>();
            
            LoadScene(ConstantValues.SCENE_NAME_LEVEL, PrepareLevelScene);
        }

        private void LoadScene(string sceneName, Action<AsyncOperation> prepareScene)
        {
            _nameNewActiveScene = sceneName;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += prepareScene;
        }

        private void PrepareLevelScene(AsyncOperation obj)
        {
            string oldSceneName = SceneManager.GetActiveScene().name;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_nameNewActiveScene));
            SceneManager.UnloadSceneAsync(oldSceneName);
            Debug.Log("New active scene : " + SceneManager.GetActiveScene().name);

            InitializerLevel initializerLevel = new GameObject().AddComponent<InitializerLevel>();
            initializerLevel.name = nameof(InitializerLevel);
            initializerLevel.Construct(staticDataService,
                localizationService,
                progressProviderService,
                processingAdsService,
                processingAnalyticsService,
                gameplayFactory,
                uiFactory);
            initializerLevel.Initialize(updateHandler);

            Debug.Log("Level scene loaded.");
            gameStateMachine.Enter<GameplayState>();
        }
    }
}