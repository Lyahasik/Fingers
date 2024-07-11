using Fingers.Core.Services;

namespace Fingers.UI.Information.Services
{
    public interface IInformationService : IService
    {
        public void Initialize(InformationView informationView);
        public void ShowWarning(string text);
    }
}