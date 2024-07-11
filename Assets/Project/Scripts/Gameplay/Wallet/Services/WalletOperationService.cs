using System;
using UnityEngine;

using EmpireCafe.Core.Progress;
using EmpireCafe.Core.Services.Progress;

namespace EmpireCafe.Gameplay.Wallet.Services
{
    public class WalletOperationService : IWalletOperationService, IWritingProgress
    {
        private IProgressProviderService _progressProviderService;

        private WalletData _walletData;

        public int Money1 => _walletData.Money1;

        public int Money2 => _walletData.Money2;

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
                    _walletData.Money1 += value;
                    break;
                case CurrencyType.Currency2:
                    _walletData.Money2 += value;
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
                    _walletData.Money1 = Math.Max(_walletData.Money1 - value, 0);
                    break;
                case CurrencyType.Currency2:
                    _walletData.Money2 = Math.Max(_walletData.Money2 - value, 0);
                    break;
            }
            
            WriteProgress();
        }

        public bool IsEnoughMoney(CurrencyType currencyType, in int value)
        {
            switch (currencyType)
            {
                case CurrencyType.Currency1:
                    return _walletData.Money1 >= value;
                case CurrencyType.Currency2:
                    return _walletData.Money2 >= value;
            }

            return true;
        }
    }
}