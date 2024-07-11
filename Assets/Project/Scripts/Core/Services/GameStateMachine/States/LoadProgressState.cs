using Fingers.Core.Services.Progress;
using UnityEngine;

namespace Fingers.Core.Services.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IProgressProviderService progressProviderService;

        public LoadProgressState(IProgressProviderService progressProviderService)
        {
            this.progressProviderService = progressProviderService;
        }

        public void Enter()
        {
            Debug.Log($"Start state { GetType().Name }");
            
            progressProviderService.StartLoadData();
        }

        public void Exit() {}
    }
}