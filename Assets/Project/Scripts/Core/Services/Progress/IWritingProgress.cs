namespace EmpireCafe.Core.Services.Progress
{
    public interface IWritingProgress : IReadingProgress
    {
        public void WriteProgress();
    }
}