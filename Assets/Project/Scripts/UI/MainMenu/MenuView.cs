using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;
using Fingers.UI.Hud;
using UnityEngine;

namespace Fingers.UI.MainMenu
{
    public class MenuView : MonoBehaviour, IWindow
    {
        [SerializeField] private WindowType windowType;

        [Space]
        [SerializeField] private WalletView walletView;

        [SerializeField] private ResultScoresView resultScoresView;
        [SerializeField] private GameObject promptStartGame;

        public void Initialize(IStaticDataService staticDataService, IProgressProviderService progressProviderService)
        {
            Debug.Log($"[{ GetType() }] initialize");
            
            walletView.Initialize(progressProviderService);
            
            resultScoresView.Construct(staticDataService);
            resultScoresView.Initialize(progressProviderService);
        }

        private void OnDisable()
        {
            promptStartGame.SetActive(false);
        }

        public void ActivationUpdate(WindowType type)
        {
            gameObject.SetActive(type == windowType);
        }
    }
}