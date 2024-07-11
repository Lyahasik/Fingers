using UnityEngine;

namespace EmpireCafe.Core.Services.StaticData
{
    [CreateAssetMenu(fileName = "StartProgressData", menuName = "Static data/Start progress")]
    public class StartProgressStaticData : ScriptableObject
    {
        public int money1;
        public int money2;
    }
}