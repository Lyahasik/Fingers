using EmpireCafe.Core.Progress;
using EmpireCafe.Core.Services.Scene;

namespace EmpireCafe.Core.Services.Progress
{
    public interface IProgressProviderService : IService
    {
        public ProgressData ProgressData { get; }
        public ISceneProviderService SceneProviderService { set; }
        public void StartLoadData();
        public void LoadProgress(string json);
        public void SaveProgress();
        public void Register(IReadingProgress progressReader);
        public void Register(IWritingProgress progressWriter);
        public void Unregister(IReadingProgress progressReader);
        public void Unregister(IWritingProgress progressWriter);
        public void WasChange();
    }
}