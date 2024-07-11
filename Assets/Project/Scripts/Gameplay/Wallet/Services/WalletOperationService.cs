using System;
using Fingers.Core.Progress;
using Fingers.Core.Services.Progress;
using UnityEngine;

namespace Fingers.Gameplay.Wallet.Services
{
    public class WalletOperationService : IWalletOperationService, IWritingProgress
    {
        private IProgressProviderService _progressProviderService;

        private WalletData _walletData;

        public int Money => _walletData.Money;

        public void Construct(IProgressProviderService progressProviderService)
        {
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            Register(_progressProviderService);
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void AddMoney(in CurrencyType currencyType, in int value)
        {
            switch (currencyType)
            {
                case CurrencyType.Currency1:
                    _walletData.Money += value;
                    break;
            }
            
            WriteProgress();
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress) => 
            _walletData = progress.Wallet;

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress() => 
            _progressProviderService.SaveProgress();

        public void RemoveMoney(CurrencyType currencyType, in int value)
        {
            switch (currencyType)
            {
                case CurrencyType.Currency1:
                    _walletData.Money = Math.Max(_walletData.Money - value, 0);
                    break;
            }
            
            WriteProgress();
        }

        public bool IsEnoughMoney(CurrencyType currencyType, in int value)
        {
            switch (currencyType)
            {
                case CurrencyType.Currency1:
                    return _walletData.Money >= value;
            }

            return true;
        }
    }
}