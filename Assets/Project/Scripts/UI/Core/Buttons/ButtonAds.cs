﻿using System;
using Fingers.Core.Publish.Services.Ads;
using UnityEngine;
using UnityEngine.UI;

namespace Fingers.UI.Core.Buttons
{
    public class ButtonAds : MonoBehaviour
    {
        [Space]
        [SerializeField] private int rewardId;
        [SerializeField] private bool isDisposable;
        [SerializeField] private Button button;

        private IProcessingAdsService _processingAdsService;

        private Action _onReward;

        public Button Button => button;

        public void Construct(IProcessingAdsService processingAdsService)
        {
            _processingAdsService = processingAdsService;
        }

        public void Initialize(Action onReward)
        {
            _onReward = onReward;

            button.onClick.AddListener(ShowAds);
            
            Subscribe();
        }

        public void UpdateRewardData(Action onReward, int newRewardId)
        {
            _onReward = onReward;
            rewardId = newRewardId;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _processingAdsService.OnClaimReward += ClaimReward;
        }

        private void Unsubscribe()
        {
            _processingAdsService.OnClaimReward -= ClaimReward;
        }

        private void ShowAds()
        {
            _processingAdsService.ShowAdsReward(rewardId);
        }

        private void ClaimReward(int rewardId)
        {
            if (this.rewardId != rewardId)
                return;
            
            if (isDisposable)
                gameObject.SetActive(false);
            
            _onReward?.Invoke();
        }
    }
}