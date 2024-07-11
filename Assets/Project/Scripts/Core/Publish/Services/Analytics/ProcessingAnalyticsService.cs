using Fingers.Core.Publish.Web.Yandex;
using Fingers.Helpers;
using UnityEngine;

namespace Fingers.Core.Publish.Services.Analytics
{
    public class ProcessingAnalyticsService : IProcessingAnalyticsService
    {
        private AnalyticsModule _analyticsModule;

        public void Initialize()
        {
            if (!OSManager.IsEditor())
                _analyticsModule = new YandexAnalyticsModule();
            
            Debug.Log($"[{GetType()}] initialize");
        }

        public void TargetAds(int id) => 
            _analyticsModule?.TargetAds(id);

        public void TargetActivity(int totalNumber)
        {
            _analyticsModule?.TargetActivity(totalNumber);
        }
    }
}