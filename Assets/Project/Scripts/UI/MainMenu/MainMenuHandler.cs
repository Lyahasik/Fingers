using System.Collections.Generic;
using UnityEngine;

using EmpireCafe.Core.Publish.Services.Ads;
using EmpireCafe.Core.Services.Factories.UI;
using EmpireCafe.UI.Information.Services;

namespace EmpireCafe.UI.MainMenu
{
    public class MainMenuHandler : MonoBehaviour
    {
        private IUIFactory _uiFactory;
        private IProcessingAdsService _processingAdsService;
        private IInformationService _informationService;

        private List<IWindow> _windows;
        private WindowType _currentWindowType;

        public void Construct(IUIFactory uiFactory,
            IProcessingAdsService processingAdsService,
            IInformationService informationService)
        {
            _uiFactory = uiFactory;
            _processingAdsService = processingAdsService;
            _informationService = informationService;
        }

        public void Initialize()
        {
            _windows = new List<IWindow>();
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