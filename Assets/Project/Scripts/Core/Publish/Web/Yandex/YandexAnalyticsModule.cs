using System.Runtime.InteropServices;

namespace Fingers.Core.Publish.Web.Yandex
{
    public class YandexAnalyticsModule : AnalyticsModule
    {
        [DllImport("__Internal")]
        private static extern void TargetAdsExtern(int id);
        
        public override void TargetAds(int id) => 
            TargetAdsExtern(id);
    }
}