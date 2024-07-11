using TMPro;
using UnityEngine;

using EmpireCafe.Core.Publish.Services.Ads;
using EmpireCafe.Core.Services.Progress;
using EmpireCafe.Core.Services.StaticData;

namespace EmpireCafe.UI.Hud
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameLevelText;
        [SerializeField] private WalletView walletView;

        public TMP_Text NameLevelText => nameLevelText;

        public void Initialize(IStaticDataService staticDataService,
            IProcessingAdsService processingAdsService,
            IProgressProviderService progressProviderService)
        {
            Debug.Log($"[{ GetType() }] initialize");
            
            walletView.Initialize(progressProviderService);
        }
    }
}
