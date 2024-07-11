using Fingers.Core.Progress;
using Fingers.Core.Services.Progress;
using TMPro;
using UnityEngine;

namespace Fingers.UI.Hud
{
    public class WalletView : MonoBehaviour, IReadingProgress
    {
        [SerializeField] private TMP_Text textMoney;

        public void Initialize(IProgressProviderService progressProviderService)
        {
            Register(progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            UpdateMoney(progress.Wallet.Money);
        }

        public void UpdateProgress(ProgressData progress)
        {
            UpdateMoney(progress.Wallet.Money);
        }

        private void UpdateMoney(int value) => 
            textMoney.text = value.ToString();
    }
}