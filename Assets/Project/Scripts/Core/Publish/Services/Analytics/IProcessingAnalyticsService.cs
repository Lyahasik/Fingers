using EmpireCafe.Core.Services;

namespace EmpireCafe.Core.Publish.Services.Analytics
{
    public interface IProcessingAnalyticsService : IService
    {
        void TargetAds(int id);
        void TargetActivity(int totalNumber);
    }
}