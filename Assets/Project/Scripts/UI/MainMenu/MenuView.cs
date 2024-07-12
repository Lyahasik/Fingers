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
    public class MenuView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private WalletView walletView;
        [SerializeField] private LocaleDropdown localeDropdown;

        [SerializeField] private ResultScoresView resultScoresView;
        [SerializeField] private GameObject promptStartGame;
        
        [Space]
        [SerializeField] private MMF_Player feedbackStartGame;
        [SerializeField] private MMF_Player feedbackEndGame;
        
        private GameplayHandler _gameplayHandler;

        public GameplayHandler GameplayHandler
        {
            set => _gameplayHandler = value;
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
            _gameplayHandler.StartGame();
        }

        public void EndGame()
        {
            feedbackEndGame.PlayFeedbacks();
        }
    }
}