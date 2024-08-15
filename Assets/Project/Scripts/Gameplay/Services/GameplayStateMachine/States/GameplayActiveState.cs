using Fingers.Core.Services.GameStateMachine.States;
using Fingers.UI.Gameplay;
using UnityEngine;

namespace Fingers.Core.Services.GameStateMachine
{
    public class GameplayActiveState : IState
    {
        private readonly GameplayHandler gameplayHandler;

        public GameplayActiveState(GameplayHandler gameplayHandler)
        {
            this.gameplayHandler = gameplayHandler;
        }

        public void Enter()
        {
            Debug.Log($"Start state { GetType().Name }");
            
            gameplayHandler.StartGame();
        }

        public void Exit() {}
    }
}