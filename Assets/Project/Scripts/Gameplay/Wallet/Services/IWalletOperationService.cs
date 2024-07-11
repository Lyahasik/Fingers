using EmpireCafe.Core.Services;

namespace EmpireCafe.Gameplay.Wallet.Services
{
    public interface IWalletOperationService : IService
    {
        public int Money1 { get; }
        public int Money2 { get; }
        
        public void Initialize();
        public void AddMoney(in CurrencyType currencyType, in int value);
        public void RemoveMoney(CurrencyType currencyType, in int value);
        public bool IsEnoughMoney(CurrencyType currencyType, in int value);
    }
}