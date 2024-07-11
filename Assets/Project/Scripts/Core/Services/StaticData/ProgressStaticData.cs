using System.Collections.Generic;
using UnityEngine;

namespace Fingers.Core.Services.StaticData
{
    [CreateAssetMenu(fileName = "ProgressData", menuName = "Static data/Progress")]
    public class ProgressStaticData : ScriptableObject
    {
        public List<int> medalsValue;
    }
}