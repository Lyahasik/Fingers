using Fingers.Core.Services;

namespace Fingers.Core.Publish.Services.Analytics
{
    public interface IProcessingAnalyticsService : IService
    {
        void TargetAds(int id);
    }
}