using Fingers.UI.Gameplay;

namespace Fingers.Core.Services.Factories.Gameplay
{
    public interface IGameplayFactory : IService
    {
        public GameplayHandler CreateGameplayHandler();
    }
}