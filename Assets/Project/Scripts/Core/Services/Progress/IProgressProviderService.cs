using Fingers.Core.Progress;
using Fingers.Core.Services.Scene;

namespace Fingers.Core.Services.Progress
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
        public void SetLocale(int localeId);
    }
}