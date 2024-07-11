using Fingers.Core.Publish.Services.Ads;
using Fingers.Core.Services.StaticData;
using Fingers.UI.Information.Prompts;
using UnityEngine;

namespace Fingers.UI.Information
{
    public class InformationView : MonoBehaviour
    {
        [SerializeField] private GameObject capArea;
        [SerializeField] private GameObject closeButton;

        [Space]
        [SerializeField] private WarningPrompt warningPrompt;
        
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

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