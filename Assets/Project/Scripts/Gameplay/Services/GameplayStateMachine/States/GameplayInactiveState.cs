using Fingers.Core.Services.GameStateMachine.States;
using Fingers.UI.Gameplay;
using UnityEngine;

namespace Fingers.Core.Services.GameStateMachine
{
    public class GameplayInactiveState : IState
    {
        private readonly GameplayHandler gameplayHandler;

        public GameplayInactiveState(GameplayHandler gameplayHandler)
        {
            this.gameplayHandler = gameplayHandler;
        }

        public void Enter()
        {
            Debug.Log($"Start state { GetType().Name }");
            
            gameplayHandler?.EndGame();
        }

        public void Exit() {}
    }
}