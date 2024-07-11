using System.Collections.Generic;
using Fingers.Core.Publish.Services.Ads;
using Fingers.Core.Services.Factories.UI;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;
using Fingers.UI.Information.Services;
using UnityEngine;

namespace Fingers.UI.MainMenu
{
    public class MainMenuHandler : MonoBehaviour
    {
        [SerializeField] private MenuView menuView;
        
        private IUIFactory _uiFactory;
        private IProcessingAdsService _processingAdsService;
        private IInformationService _informationService;

        private List<IWindow> _windows;
        private WindowType _currentWindowType;

        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        public void Construct(IUIFactory uiFactory,
            IProcessingAdsService processingAdsService,
            IInformationService informationService)
        {
            _uiFactory = uiFactory;
            _processingAdsService = processingAdsService;
            _informationService = informationService;
        }

        public void Initialize(IStaticDataService staticDataService, IProgressProviderService progressProviderService)
        {
            _windows = new List<IWindow>();
            
            _windows.Add(menuView);
            
            menuView.Initialize(staticDataService, progressProviderService);
        }

        public void ActivateWindow(int idWindow)
        {
            _currentWindowType = (WindowType) idWindow;

            foreach (IWindow window in _windows) 
                window.ActivationUpdate(_currentWindowType);
            
            _processingAdsService.ShowAdsInterstitial();
        }
    }
}