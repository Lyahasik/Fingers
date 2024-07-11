using UnityEngine;

namespace EmpireCafe.Core.Services.GameStateMachine.States
{
    public class GameplayState : IState
    {
        public void Enter()
        {
            Debug.Log($"Start state {GetType().Name}");
        }

        public void Exit() {}
    }
}