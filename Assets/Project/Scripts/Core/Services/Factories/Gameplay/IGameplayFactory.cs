using UnityEngine;

namespace EmpireCafe.Core.Services.Factories.Gameplay
{
    public interface IGameplayFactory : IService
    {
        public Canvas CreateGameplayCanvas();
    }
}