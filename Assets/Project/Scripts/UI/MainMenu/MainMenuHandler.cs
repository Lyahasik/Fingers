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

        private IProcessingAdsService _processingAdsService;
        private IInformationService _informationService;

        private List<IWindow> _windows;
        private WindowType _currentWindowType;

        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        public void Construct(IProcessingAdsService processingAdsService,
            IInformationService informationService)
        {
            _processingAdsService = processingAdsService;
            _informationService = informationService;
        }

        public void Initialize(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IProgressProviderService progressProviderService)
        {
            _windows = new List<IWindow>();
            
            _windows.Add(menuView);
            
            menuView.Initialize(staticDataService, localizationService, progressProviderService);
        }

        public void ActivateMenu()
        {
            menuView.EndGame();
        }

        public void DeactivateMenu()
        {
            menuView.DeactivateMenu();
        }

        public void ActivateWindow(int idWindow)
        {
            _currentWindowType = (WindowType) idWindow;

            foreach (IWindow window in _windows) 
                window.ActivationUpdate(_currentWindowType);
            
            _processingAdsService.ShowAdsInterstitial();
        }

        public void SetGameplayHandler(GameplayHandler gameplayHandler)
        {
            menuView.GameplayHandler = gameplayHandler;
        }
    }
}