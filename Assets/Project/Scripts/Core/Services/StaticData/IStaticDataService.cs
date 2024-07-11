using EmpireCafe.UI.StaticData;

namespace EmpireCafe.Core.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        public StartProgressStaticData StartProgress { get; }
        public UIStaticData UI { get; }
    }
}