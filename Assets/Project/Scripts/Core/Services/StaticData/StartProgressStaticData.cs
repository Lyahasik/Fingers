using UnityEngine;

namespace Fingers.Core.Services.StaticData
{
    [CreateAssetMenu(fileName = "StartProgressData", menuName = "Static data/Start progress")]
    public class StartProgressStaticData : ScriptableObject
    {
        public int money;
    }
}