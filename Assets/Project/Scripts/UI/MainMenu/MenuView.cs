using Fingers.Core.Progress;
using Fingers.Core.Publish.Services.Ads;
using Fingers.Core.Services.GameStateMachine;
using Fingers.Core.Services.Localization;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;
using Fingers.UI.Gameplay;
using Fingers.UI.Hud;
using Fingers.UI.Localization;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Fingers.UI.MainMenu
{
    public class MenuView : MonoBehaviour, IWindow, IReadingProgress
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private WalletView walletView;
        [SerializeField] private LocaleDropdown localeDropdown;

        [SerializeField] private ResultScoresView resultScoresView;
        [SerializeField] private GameObject promptStartGame;

        [Space]
        [SerializeField] private GameObject cap;
        [SerializeField] private ReplayWindow replayWindow;
        [SerializeField] private GameObject reviewWindow;
        
        [Space]
        [SerializeField] private MMF_Player feedbackStartGame;
        [SerializeField] private MMF_Player feedbackEndGame;
        [SerializeField] private MMF_Player feedbackOpenMenu;

        private IProcessingAdsService _processingAdsService;
        private GameplayHandler _gameplayHandler;

        private int _recordScores;
        private int _currentScores;
        private bool _isReplay;
        private bool _isEndGame;

        public GameplayHandler GameplayHandler
        {
            set => _gameplayHandler = value;
        }

        public void Construct(IProcessingAdsService processingAdsService)
        {
            _processingAdsService = processingAdsService;
        }

        public void Initialize(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IProgressProviderService progressProviderService)
        {
            Debug.Log($"[{ GetType() }] initialize");
            
            walletView.Initialize(progressProviderService);
            
            resultScoresView.Construct(staticDataService, localizationService);
            resultScoresView.Initialize(progressProviderService);
            
            localeDropdown.Construct(localizationService, progressProviderService);
            localeDropdown.Initialize();
            
            replayWindow.Construct(this);
            replayWindow.Initialize(_processingAdsService);
            
            Register(progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _recordScores = progress.ScoresData.RecordNumber;
        }

        public void UpdateProgress(ProgressData progress)
        {
            _recordScores = progress.ScoresData.RecordNumber;
        }

        private void OnDisable()
        {
            promptStartGame.SetActive(false);
        }

        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
        }

        public void DeactivateMenu()
        {
            feedbackStartGame.PlayFeedbacks();
        }

        public void StartGame()
        {
            _isEndGame = false;
                
            if (!_gameplayHandler.GameplayArea.IsLockPlayer)
                _gameplayHandler.ChangeState<GameplayActiveState>();
        }

        public void PauseGame(int scores)
        {
            _currentScores = scores;
            
            if (!_isReplay
                && _currentScores < _recordScores
                && _currentScores > _recordScores * 0.5f)
            {
                _isReplay = true;
                
                cap.SetActive(true);
                replayWindow.gameObject.SetActive(true);
                feedbackEndGame.PlayFeedbacks();
            }
            else
            {
                EndGame(scores);
            }
        }

        public void EndGame(int scores)
        {
            if (_isEndGame)
                return;

            _currentScores = scores;
            _isReplay = false;
            _isEndGame = true;
            
            feedbackEndGame.PlayFeedbacks();
        }

        public void ConfirmEndGame()
        {
            _isEndGame = true;
            OpenMainMenu();
        }

        public void OpenMainMenu()
        {
            if (!_isEndGame)
                return;
            
            _gameplayHandler.ChangeState<GameplayInactiveState>();
            
            cap.SetActive(false);
            replayWindow.gameObject.SetActive(false);
            reviewWindow.SetActive(false);
            
            feedbackOpenMenu.PlayFeedbacks();
            
            _processingAdsService.ShowAdsInterstitial();
        }

        public void ContinueGame()
        {
            cap.SetActive(false);
            replayWindow.gameObject.SetActive(false);
            
            _gameplayHandler.ChangeState<GameplayPrepareState>();
        }

        public void DeactivateGameplay()
        {
            _gameplayHandler.DeactivateGameplay();
        }
    }
}