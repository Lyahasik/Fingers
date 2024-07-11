using System;
using UnityEngine;

using EmpireCafe.Core.Publish.Services.Analytics;
using EmpireCafe.Core.Publish.Web.Yandex;
using EmpireCafe.Helpers;

namespace EmpireCafe.Core.Publish.Services.Ads
{
    public class ProcessingAdsService : IProcessingAdsService
    {
        private readonly IProcessingAnalyticsService processingAnalyticsService;
        
        private AdsModule _adsModule;

        private int _currentRewardId;

        public event Action<int> OnClaimReward;

        public ProcessingAdsService(IProcessingAnalyticsService processingAnalyticsService)
        {
            this.processingAnalyticsService = processingAnalyticsService;
        }

        public void Initialize()
        {
            if (!OSManager.IsEditor())
                _adsModule = new YandexAdsModule();
            
            Debug.Log($"[{GetType()}] initialize");
        }

        public void ShowAdsReward(int rewardId)
        {
            processingAnalyticsService.TargetAds(rewardId);
            _currentRewardId = rewardId;

            if (OSManager.IsEditor())
            {
                ClaimReward();
                return;
            }

            _adsModule?.ShowAdsReward();
            PrepareAds();
        }

        public void ShowAdsInterstitial()
        {
            if (_adsModule != null
                && _adsModule.TryShowAdsInterstitial())
                PrepareAds();
        }

        public void EndAds()
        {
            Time.timeScale = 1f;
        }

        public void ClaimReward()
        {
            OnClaimReward?.Invoke(_currentRewardId);
        }

        private void PrepareAds()
        {
            Time.timeScale = 0f;
        }
    }
}