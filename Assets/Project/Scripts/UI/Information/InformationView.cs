using UnityEngine;

using EmpireCafe.Core.Publish.Services.Ads;
using EmpireCafe.Core.Services.StaticData;
using EmpireCafe.UI.Information.Prompts;

namespace EmpireCafe.UI.Information
{
    public class InformationView : MonoBehaviour
    {
        [SerializeField] private GameObject capArea;
        [SerializeField] private GameObject closeButton;

        [Space]
        [SerializeField] private WarningPrompt warningPrompt;

        public void Initialize(IStaticDataService staticDataService,
            IProcessingAdsService processingAdsService)
        {
            
        }

        public void ShowWarning(string message) => 
            warningPrompt.Show(message);

        public void CloseView()
        {
            capArea.SetActive(false);
            closeButton.SetActive(false);
            
            warningPrompt.Hide();
        }

        private void CapActivate()
        {
            capArea.SetActive(true);
            closeButton.SetActive(true);
        }
    }
}