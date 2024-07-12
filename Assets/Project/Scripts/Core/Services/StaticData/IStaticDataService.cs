using Fingers.UI.StaticData;

namespace Fingers.Core.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        public StartProgressStaticData StartProgress { get; }
        public UIStaticData UI { get; }
        public ProgressStaticData Progress { get; }
        public GameplayStaticData Gameplay { get; }
    }
}