using System.Runtime.InteropServices;
using UnityEngine;

using EmpireCafe.Core.Publish.Services.Ads;
using EmpireCafe.Core.Publish.Web.Yandex;
using EmpireCafe.Core.Services.Progress;
using EmpireCafe.Helpers;

namespace EmpireCafe.Core.Publish
{
    public class PublishHandler : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void CheckRateGameExtern();
        [DllImport("__Internal")]
        private static extern void RateGameExtern();

        private IProgressProviderService _progressProviderService;
        private IProcessingAdsService _processingAdsService;

        private DataModule _dataModule;

        public void Construct(string newName)
        {
            name = newName;
        }

        public void Initialize(IProgressProviderService progressProviderService,
            IProcessingAdsService processingAdsService)
        {
            _dataModule = new YandexDataModule();

            _progressProviderService = progressProviderService;
            _processingAdsService = processingAdsService;
        }

#region data
        
        public void StartLoadData()
        {
            _dataModule?.StartLoadData();
        }
        
        public void LoadProgress(string json)
        {
            _progressProviderService.LoadProgress(json);
        }

        public void SaveData(string data)
        {
            _dataModule?.SaveData(data);
        }

        public void SetLeaderBoard(int value)
        {
            _dataModule?.SetLeaderBoard(value);
        }
        
#endregion
        
        public void ClaimRewardAds()
        {
            _processingAdsService.ClaimReward();
            EndAds();
        }
        
        public void EndAds()
        {
            _processingAdsService.EndAds();
        }

        public void StartCheckRateGame()
        {
            if (!OSManager.IsEditor())
                CheckRateGameExtern();
        }

        private void StartRateGame()
        {
            if (!OSManager.IsEditor())
                RateGameExtern();
        }
    }
}
