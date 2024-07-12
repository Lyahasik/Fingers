using Fingers.Constants;
using Fingers.Gameplay.StaticData;
using Fingers.Gameplay.Wallet;
using Fingers.UI.StaticData;
using UnityEngine;

namespace Fingers.Core.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private StartProgressStaticData _startProgress; 
        private UIStaticData _ui;
        private ProgressStaticData _progress;
        private GameplayStaticData _gameplay;

        private MoneysStaticData _moneys;

        public StartProgressStaticData StartProgress => _startProgress;
        public UIStaticData UI => _ui;
        public ProgressStaticData Progress => _progress;
        public GameplayStaticData Gameplay => _gameplay;

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
            
            _progress = Resources
                .Load<ProgressStaticData>(ConstantPaths.PROGRESS_DATA_PATH);
            
            _gameplay = Resources
                .Load<GameplayStaticData>(ConstantPaths.GAMEPLAY_DATA_PATH);
            
            _moneys = Resources
                .Load<MoneysStaticData>(ConstantPaths.MONEYS_DATA_PATH);
        }
        
        public Sprite GetIconMoneyByType(CurrencyType currencyType) => 
            _moneys.moneysData.Find(data => data.type == currencyType).icon;
    }
}