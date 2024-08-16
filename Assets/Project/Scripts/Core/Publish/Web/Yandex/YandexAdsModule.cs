using System.Runtime.InteropServices;
using Fingers.Constants;
using Fingers.Helpers;
using UnityEngine;

namespace Fingers.Core.Publish.Web.Yandex
{
    public class YandexAdsModule : AdsModule
    {
        [DllImport("__Internal")]
        private static extern void ShowAdsInterstitialExtern();
    
        [DllImport("__Internal")]
        private static extern void ShowAdsRewardExtern();
        
        [DllImport("__Internal")]
        private static extern void TargetAdsExtern();

        public YandexAdsModule()
        {
            _nextBlockAdsTime = Time.time + ConstantValues.ADS_BLOCK_DELAY_TIME;
        }

        public override bool TryShowAdsInterstitial()
        {
            if (OSManager.IsEditor()
                || _nextBlockAdsTime > Time.time)
                return false;
            
            ShowAdsInterstitialExtern();
            _nextBlockAdsTime = Time.time + ConstantValues.ADS_BLOCK_DELAY_TIME;
            
            return true;
        }

        public override void ShowAdsReward()
        {
            ShowAdsRewardExtern();
            TargetAdsExtern();
        }

        public override void ShowAdsReward(int rewardId)
        {
            ShowAdsRewardExtern();
        }
    }
}
