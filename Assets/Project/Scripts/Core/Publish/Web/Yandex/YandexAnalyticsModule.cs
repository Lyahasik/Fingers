using System.Runtime.InteropServices;

namespace EmpireCafe.Core.Publish.Web.Yandex
{
    public class YandexAnalyticsModule : AnalyticsModule
    {
        [DllImport("__Internal")]
        private static extern void TargetAdsExtern(int id);
        [DllImport("__Internal")]
        private static extern void TargetActivityExtern(int number);
        
        public override void TargetAds(int id) => 
            TargetAdsExtern(id);

        public override void TargetActivity(int number) => 
            TargetActivityExtern(number);
    }
}