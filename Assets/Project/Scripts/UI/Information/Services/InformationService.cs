using UnityEngine;

namespace Fingers.UI.Information.Services
{
    public class InformationService : IInformationService
    {
        private InformationView _informationView;

        public void Initialize(InformationView informationView)
        {
            _informationView = informationView;
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void ShowWarning(string message)
        {
            _informationView.ShowWarning(message);
        }
    }
}