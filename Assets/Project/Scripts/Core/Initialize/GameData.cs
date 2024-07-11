using UnityEngine;

using EmpireCafe.Core.Coroutines;
using EmpireCafe.Core.Services;
using EmpireCafe.UI.Loading;

namespace EmpireCafe.Core.Initialize
{
    public class GameData : MonoBehaviour
    {
        private LoadingCurtain _curtain;
        private CoroutinesContainer _coroutinesContainer;
        private ServicesContainer _servicesContainer;

        public ServicesContainer ServicesContainer => _servicesContainer;
        public CoroutinesContainer CoroutinesContainer => _coroutinesContainer;
        public LoadingCurtain Curtain => _curtain;

        public void Construct(LoadingCurtain curtain,
            CoroutinesContainer coroutinesContainer, ServicesContainer servicesContainer)
        {
            _curtain = curtain;
            _coroutinesContainer = coroutinesContainer;
            _servicesContainer = servicesContainer;
        }
    }
}