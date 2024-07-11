namespace Fingers.Core.Services.Progress
{
    public interface IWritingProgress : IReadingProgress
    {
        public void WriteProgress();
    }
}