using Fingers.Core.Services;

namespace Fingers.Gameplay.Wallet.Services
{
    public interface IWalletOperationService : IService
    {
        public int Money { get; }
        
        public void Initialize();
        public void AddMoney(in CurrencyType currencyType, in int value);
        public void RemoveMoney(CurrencyType currencyType, in int value);
        public bool IsEnoughMoney(CurrencyType currencyType, in int value);
    }
}