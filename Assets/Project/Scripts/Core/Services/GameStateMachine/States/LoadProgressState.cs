using UnityEngine;

using EmpireCafe.Core.Services.Progress;

namespace EmpireCafe.Core.Services.GameStateMachine.States
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