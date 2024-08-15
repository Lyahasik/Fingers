using Fingers.Core.Publish.Services.Ads;
using Fingers.UI.Core.Buttons;
using UnityEngine;

namespace Fingers.UI.MainMenu
{
    public class ReplayWindow : MonoBehaviour
    {
        [SerializeField] private ButtonAds buttonAds;

        private MenuView _menuView;

        public void Construct(MenuView menuView)
        {
            _menuView = menuView;
        }

        public void Initialize(IProcessingAdsService processingAdsService)
        {
            buttonAds.Construct(processingAdsService);
            buttonAds.Initialize(ContinueGame);
        }

        private void ContinueGame()
        {
            _menuView.ContinueGame();
        }
    }
}