using System.Collections.Generic;
using UnityEngine;

namespace EmpireCafe.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "MoneysData", menuName = "Static data/Moneys")]
    public class MoneysStaticData : ScriptableObject
    {
        public List<MoneyData> moneysData;
    }
}