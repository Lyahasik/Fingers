using UnityEngine;

using EmpireCafe.Constants;
using EmpireCafe.Gameplay.StaticData;
using EmpireCafe.Gameplay.Wallet;
using EmpireCafe.UI.StaticData;

namespace EmpireCafe.Core.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private StartProgressStaticData _startProgress; 
        private UIStaticData _ui;
        
        private MoneysStaticData _moneys;

        public StartProgressStaticData StartProgress => _startProgress;
        public UIStaticData UI => _ui;

        public StaticDataService()
        {
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void Load()
        {
            _startProgress = Resources
                .Load<StartProgressStaticData>(ConstantPaths.START_PROGRESS_DATA_PATH);
            
            _ui = Resources
                .Load<UIStaticData>(ConstantPaths.UI_DATA_PATH);
            
            _moneys = Resources
                .Load<MoneysStaticData>(ConstantPaths.MONEYS_DATA_PATH);
        }
        
        public Sprite GetIconMoneyByType(CurrencyType currencyType) => 
            _moneys.moneysData.Find(data => data.type == currencyType).icon;
    }
}