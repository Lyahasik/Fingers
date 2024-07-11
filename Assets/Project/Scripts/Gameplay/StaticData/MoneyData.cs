using System;
using UnityEngine;

using EmpireCafe.Gameplay.Wallet;

namespace EmpireCafe.Gameplay.StaticData
{
    [Serializable]
    public class MoneyData
    {
        public CurrencyType type;
        public Sprite icon;
    }
}