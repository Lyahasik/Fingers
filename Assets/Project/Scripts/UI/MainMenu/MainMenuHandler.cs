using System.Collections.Generic;
using Fingers.Core.Publish.Services.Ads;
using Fingers.Core.Services.Localization;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;
using Fingers.UI.Gameplay;
using Fingers.UI.Information.Services;
using UnityEngine;

namespace Fingers.UI.MainMenu
{
    public class MainMenuHandler : MonoBehaviour
    {
        [SerializeField] private MenuView menuView;

        private IInformationService _informationService;

        private List<IWindow> _windows;
        private WindowType _currentWindowType;

        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        public void Construct(IInformationService informationService)
        {
            _informationService = informationService;
        }

        public void Initialize(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IProcessingAdsService processingAdsService,
            IProgressProviderService progressProviderService)
        {
            _windows = new List<IWindow>();
            
            _windows.Add(menuView);
            
            menuView.Construct(processingAdsService);
            menuView.Initialize(staticDataService, localizationService, progressProviderService);
        }

        public void ActivateMenu(int scores)
        {
            menuView.EndGame(scores);
        }

        public void PauseGame(int scores)
        {
            menuView.PauseGame(scores);
        }

        public void DeactivateMenu()
        {
            menuView.DeactivateMenu();
        }

        public void SetGameplayHandler(GameplayHandler gameplayHandler)
        {
            menuView.GameplayHandler = gameplayHandler;
        }
    }
}